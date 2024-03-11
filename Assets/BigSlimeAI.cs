using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class BigSlimeAI : MonoBehaviour
{
    [SerializeField] private GameObject shockwave;
    [SerializeField] private GameObject circleExpand;
    [SerializeField] private Transform firingPointParent;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private GameObject projectilePrefab;
    private Vector2 startPos; //-0.45, 32.4 for holy +80 for void
    Vector2 newPos;
    Transform shockwaveParent;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        //shockwave = GameObject.Find("Shockwave(Clone)");
        //shockwave.SetActive(false);
        if (GameManager.Instance.isGamePlaying){
            if(gameObject.name.Contains("Holy")){
                newPos = new Vector2(-0.45f, -4.5f); //we are only changing Y here so just get X when we do other arenas
                shockwaveParent = GameObject.Find("Holy").transform;
            }
            else if (gameObject.name.Contains("Void")){
                newPos = new Vector2(79.45f, -4.5f);
                shockwaveParent = GameObject.Find("Void").transform;
            }
            var sequence = DOTween.Sequence();
            sequence.Append(transform.DOMove(newPos , 1f).SetEase(Ease.Flash).OnComplete(() => {
                Instantiate(Resources.Load<GameObject>("Prefabs/VFX/Shockwave"), shockwaveParent.localPosition, Quaternion.identity, shockwaveParent);
                CinemachineShake.Instance.ShakeCamera(4, 0.2f);
                ShootAOEPulse();
            }));
            sequence.Append(circleExpand.transform.DOScale(0.34f, 5));//expand circle
            sequence.Join(circleExpand.GetComponent<SpriteRenderer>().DOColor(new Color(1, 0.82f, 0, 0.3f), 5));//colour circle
            sequence.Join(circleExpand.transform.DORotate(new Vector3(0,0,-180), 5));
            sequence.Append(transform.DOShakePosition(1f, new Vector3(0.5f, 0, 0), 5, 0).SetEase(Ease.Flash).OnComplete(() => {
                if (GameManager.Instance.shouldSpawnHoly && gameObject.name.Contains("Holy") || !GameManager.Instance.shouldSpawnHoly && gameObject.name.Contains("Void")){
                    GameManager.Instance.KillPlayer();
                }
                circleExpand.transform.localScale = new Vector3(0,0,0);
            }));
            sequence.Append(transform.DOMove(startPos, 1f).SetEase(Ease.Flash).OnComplete(() => {
                    Destroy(gameObject, 1f);
            }));
        }
        
    }

    private void Update() {
        if (!GameManager.Instance.isGamePlaying){
            Destroy(gameObject);
        }
    }

    private void OnDestroy() {
        transform.DOKill();
    }

    public void ShootAOEPulse(){
        Vector3 rotation = new Vector3(0,1,0);
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        firingPointParent.rotation = Quaternion.Euler(0, 0, rotZ);
        float angle = 15f;
        for (int i = 0; i < 24; i++){
            var shotRotation = firingPointParent.rotation;
            
            shotRotation *= Quaternion.Euler(0,0,angle);
            
            Instantiate(projectilePrefab, firingPointParent.position, shotRotation);
            angle -= 15f;
        }
        /*
        if (colour == "Holy"){
            Vector3 rotation = new Vector3(0,0,0);
            float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg - 90f;
            firingPointParent.rotation = Quaternion.Euler(0, 0, rotZ);
            float angle = 30f;
            for (int i = 0; i < 12; i++){
                var shotRotation = transform.rotation;
                shotRotation *= Quaternion.Euler(0,0,angle);
                Instantiate(projectilePrefab, firingPoint.position, shotRotation);
                angle -= 30f;
            }
        }
        else{
            Vector3 rotation = new Vector3(0,0,0);
            float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, rotZ);
            float angle = 30f;
            for (int i = 0; i < 12; i++){
                var shotRotation = transform.rotation;
                shotRotation *= Quaternion.Euler(0,0,angle);
                Instantiate(projectilePrefab, bulletTransform.position, shotRotation);
                angle -= 30f;
            }
        }*/
    }
}
