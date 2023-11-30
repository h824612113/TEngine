using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using TEngine;
using UniFramework.Utility;
using UnityEngine;
using UnityEngine.PlayerLoop;
using AudioType = TEngine.AudioType;

namespace GameLogic
{
    [Update]
    public class GameSystem : BehaviourSingleton<GameSystem>
    {
        private enum ESTeps
        {
            None,
            Ready,
            Spawn,
            WaitSpawn,
            GameOver
        }

        private Spawner _entitySpawner;
        private GameObject _roomRoot;

        private ESTeps _steps = ESTeps.None;
        private int _totalScore = 0;


        private UniTimer _startWaitTimer = UniTimer.CreateOnceTimer(0.5f);
        private UniTimer _spawnWaitTimer = UniTimer.CreateOnceTimer(7.5f);

        private float Bg_MoveSpeed = 0.5f;

        private float Bg_SpawnPositionX = -29.11f;//需要生成新的bg的位置
        private float Bg_HidePositionX = -46.86f;//需要隐藏旧的bg的位置
        private float Bg_Spawn_InitPosX = -3.4f;//初始化bg的位置
        
        private int Max_BgCount = 3;//同时存在最大bg预制数


        #region pipe

        /// <summary>
        /// 管道产生时间
        /// </summary>
        private float m_PipeSpawnTime = 0f;

        private UniTimer _pipeSpawnTimer;
        private float Pipe_MoveSpeed = 0.5f;
        private const float Pipe_Left = -35f;
        private const float Pipe_Left_Init = -25f; //初始管道生成的位置
        private const float Pipe_Right = -17.8f;
        private float Pipe_HidePositionX = -35.9f;
        private int Pipe_TotalCount = 0;//初始管道个数
        private Vector3 lastPipePostion;//最后一个管道的位置

        private Queue<SpawnHandle> allPipeQueue = new Queue<SpawnHandle>();//所有的管道队列

        private const int PerSpawn_PipeCount = 5;//同时产生管道的个数
        #endregion

        private bool isGameOver = false;
        public IEnumerator LoadRoom()
        {
            Time.timeScale = 1.0f;
            //创建房间根对象
            _roomRoot = new GameObject("GameRoom");
            //加载背景音乐
            GameModule.Audio.Play(AudioType.Music, FlappySoundMusicConfig.MAIN, true);
            //创建游戏对象发生器
            _entitySpawner = ResourcePool.CreateSpawner("DefaultPackage");

            //创建游戏对象池
            yield return _entitySpawner.CreateGameObjectPoolAsync("Bg");

            yield return _entitySpawner.CreateGameObjectPoolAsync("Pipe");
            yield return _entitySpawner.CreateGameObjectPoolAsync("Bird");
            yield return _entitySpawner.CreateGameObjectPoolAsync("Bullet");
            

            //创建玩家对象
            Vector3 spawnPosition = new Vector3(-25.7f, 0.5f, 0);
            Quaternion spawnRotation = Quaternion.identity;
            BgData bgData = new BgData(Bg_MoveSpeed,Bg_SpawnPositionX,Bg_HidePositionX);
            var handle = _entitySpawner.SpawnSync("Bg", _roomRoot.transform, spawnPosition, spawnRotation,false,bgData);
            var entity = handle.GameObj.GetComponent<BgEntity>();
            entity.InitEntity(handle);
            FlappyModel.Instance.TotalBgCount += 1;

            //创建Bird
            BirdData birdData = new BirdData(3f);
            spawnPosition = new Vector3(Pipe_Left+1, 0, 0);
            var handle1 = _entitySpawner.SpawnSync("Bird", _roomRoot.transform, spawnPosition, spawnRotation,false,birdData);
            var entity1 = handle1.GameObj.GetComponent<BirdEntity>();
            entity1.InitEntity(handle1);
            //创建管道

            //初始创建5-8个
            //然后每消失一个创建一个
            Pipe_TotalCount = Random.Range(5, 8);
            
            SpawanPipesByCount(Pipe_TotalCount);

            lastPipePostion = new Vector3(Pipe_Right,1.0f,-1);
            //显示战斗界面



            // _startWaitTimer.Reset();
            _steps = ESTeps.Ready;
            isGameOver = false;
            
            //事件
            GameEvent.AddEventListener<Vector3, Quaternion>(FlappyEventDefine.FlappyBgCreate, OnSpawnBg);
            GameEvent.AddEventListener(FlappyEventDefine.FlappyPipeCreate,OnSpawnPipe);
            GameEvent.AddEventListener(FlappyEventDefine.FlappyGameOver,OnGameOver);
            GameEvent.AddEventListener(FlappyEventDefine.ScoreChange, OnScoreChange);
        }
        
        /// <summary>
        /// 生成指定个数的管道
        /// </summary>
        /// <param name="pipeCount"></param>
        /// <param name="initPosX"></param>
        private void SpawanPipesByCount(int pipeCount,float initPosX = Pipe_Left_Init,bool isFirst = true)
        {
            
            // Pipe_TotalCount = pipeInitCount;
            float screenX = Mathf.Abs(Pipe_Left - Pipe_Right)+3;//UnityEngine.Screen.width;
            float sectionRandomX;
            float upOffset;
            float downOffset;
            float hideTarget;
            Debug.Log("获取当前屏幕的宽度======"+screenX);
            // float randomX =
            float sectionDistance = screenX / Pipe_TotalCount * 1.0f;
            for (int i = 0; i < pipeCount; i++)
            {
                sectionRandomX = Random.Range(sectionDistance * 0.05f, sectionDistance * 0.1f);
                Debug.Log("初始位置是------"+sectionRandomX+"====最终位置是-----"+i*sectionDistance+sectionRandomX);
                Vector3 pipeSpawnPosition;
                if (isFirst)
                {
                    pipeSpawnPosition = new Vector3(initPosX+sectionRandomX+sectionDistance*i, 1.0f, -1);
                }
                else
                {
                    pipeSpawnPosition = new Vector3(initPosX+sectionRandomX+sectionDistance*(i+1), 1.0f, -1);
                }
                upOffset = Random.Range(3.0f, 6.5f);
                downOffset = Random.Range(-4f, -2f);
                hideTarget = Bg_HidePositionX;
                PipeData data = new PipeData(Pipe_MoveSpeed, upOffset, downOffset,Pipe_HidePositionX);
                // Vector3 worldPos = Camera.main.ScreenToWorldPoint(pipeSpawnPosition);
                Debug.Log("当前的pipe的的位置-----"+i+"======="+pipeSpawnPosition);
                
                var handle = _entitySpawner.SpawnSync("Pipe", _roomRoot.transform,pipeSpawnPosition, Quaternion.identity,false,data);
                var pipeEntity = handle.GameObj.GetComponent<PipeEntity>();
                pipeEntity.InitEntity(handle);
                allPipeQueue.Enqueue(handle);
                
            }
        }

        public void DestroyRoom()
        {
            GameModule.Audio.Stop(AudioType.Music,true);
            if (_entitySpawner != null)
                _entitySpawner.DestroyAll(true);

            if (_roomRoot != null)
                Object.Destroy(_roomRoot);
            
            GameModule.UI.CloseWindow<GameOverForm>();
            FlappyModel.Instance.TotalBgCount = 0;
            FlappyModel.Instance.TotalScore = 0;
            Pipe_TotalCount = 0;
            allPipeQueue.Clear();
            //移除事件
            GameEvent.RemoveEventListener<Vector3,Quaternion>(FlappyEventDefine.FlappyBgCreate,OnSpawnBg);
            GameEvent.RemoveEventListener(FlappyEventDefine.FlappyPipeCreate,OnSpawnPipe);
            GameEvent.RemoveEventListener(FlappyEventDefine.FlappyGameOver,OnGameOver);
            GameEvent.RemoveEventListener(FlappyEventDefine.ScoreChange, OnScoreChange);
        }

        #region 接收事件

        private void OnSpawnBg(Vector3 position,Quaternion rotation)
        {
            if (isGameOver)
            {
                return;
            }             
            // 创建新的bg
            Debug.Log("创建新的bg-----OnSpawnBg");
            _steps = ESTeps.Spawn;
            

        }
        
        private void OnSpawnPipe()
        {
            if(isGameOver)
                return;
            if (allPipeQueue.Count > 0)
                allPipeQueue.Dequeue();
            
             if(allPipeQueue.Count>Pipe_TotalCount)
                return;
            
            lastPipePostion = allPipeQueue.Last().GameObj.transform.position;
            SpawanPipesByCount(PerSpawn_PipeCount,lastPipePostion.x,false);
            
        }

        private void OnGameOver()
        {
            Debug.Log("游戏结束------OnGameOver--");
            GameModule.UI.ShowUI<GameOverForm>();
            _steps = ESTeps.GameOver;
            isGameOver = true;
            Time.timeScale = 0;
        }

        private void OnScoreChange()
        {
            Debug.Log("通过管道---加分---OnScoreChange--");
            
        }

        #endregion

        public override void Update()
        {
            if(isGameOver)
                return;
            UpdateRoom();
        }

        public void UpdateRoom()
        {
            if (_steps == ESTeps.None || _steps == ESTeps.GameOver)
            {
                return;
            }

            if (_steps == ESTeps.Ready)
            {
                // if (_startWaitTimer.Update(Time.deltaTime))
                // {
                //     _steps = ESTeps.Spawn;
                // }
            }

            if (_steps == ESTeps.Spawn)
            {
                if (FlappyModel.Instance.TotalBgCount >= Max_BgCount)
                {
                    _steps = ESTeps.WaitSpawn;
                    return;
                }
                //生成B
                var bgLocation = "Bg";
                Vector3 spawnPosition = new Vector3(Bg_Spawn_InitPosX, 0.5f, 0);
                Quaternion spawnRotation = Quaternion.identity;
                BgData bgData = new BgData(Bg_MoveSpeed,Bg_SpawnPositionX,Bg_HidePositionX);
                var handle = _entitySpawner.SpawnSync(bgLocation, _roomRoot.transform,spawnPosition, spawnRotation,false,bgData);
                var entity = handle.GameObj.GetComponent<BgEntity>();
                entity.InitEntity(handle);
                FlappyModel.Instance.TotalBgCount += 1;
                //创建管道
                
                _steps = ESTeps.WaitSpawn;
            }

            if (_steps == ESTeps.WaitSpawn)
            {
                Debug.Log("等待创建新的bg和管道-------------------");
            }
            // if (_steps == ESTeps.WaitSpawn)
            // {
            //     if (_spawnWaitTimer.Update(Time.deltaTime))
            //     {
            //         _spawnWaitTimer.Reset();
            //         _steps = ESTeps.Spawn;
            //     }
            // }
        }
    }
}