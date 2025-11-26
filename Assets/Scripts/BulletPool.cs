using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    private static BulletPool _instance;

    public static BulletPool Instance{
        get{
            if(_instance == null){
                _instance = FindObjectOfType<BulletPool>();
            }
            return _instance;
        }
    }

    [SerializeField] private BulletHell bulletPrefab;
    [SerializeField] private int poolSize = 50;

    private List<BulletHell> bulletPool = new List<BulletHell>(); 

    private void Awake(){
        if (_instance != null && _instance != this){
            Destroy(gameObject);
            return;
        }else{
        _instance = this;
        }

        AddBulletsToPool(poolSize);
    }

    private void AddBulletsToPool(int size){
        for(int i = 0; i < size; i++){
            BulletHell bullet = Instantiate(bulletPrefab);
            bullet.gameObject.SetActive(false);
            bulletPool.Add(bullet);
            bullet.transform.parent = transform;
        }
    }
    public BulletHell requestBullet(){
        for(int i = 0; i < bulletPool.Count; i++){
            if(!bulletPool[i].gameObject.activeSelf){
                bulletPool[i].gameObject.SetActive(true);
                return bulletPool[i];
            }
        }
        AddBulletsToPool(1);
        bulletPool[bulletPool.Count - 1].gameObject.SetActive(true);
        return bulletPool[bulletPool.Count - 1];

    }
}
