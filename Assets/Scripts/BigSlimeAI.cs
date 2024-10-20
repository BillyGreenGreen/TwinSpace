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
    float circleExpandDuration = 5;
    Vector2 newPos;
    Transform shockwaveParent;
    Sequence sequence;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        //shockwave = GameObject.Find("Shockwave(Clone)");
        //shockwave.SetActive(false);
        if (GameManager.Instance.isGamePlaying){
            if(gameObject.name.Contains("Holy")){
                newPos = new Vector2(-0.45f, -4.5f + GameManager.Instance.yOffset); //we are only changing Y here so just get X when we do other arenas
                shockwaveParent = GameObject.Find("Holy" + GameManager.Instance.arenaIndex.ToString()).transform;
            }
            else if (gameObject.name.Contains("Void")){
                newPos = new Vector2(79.45f, -4.5f + GameManager.Instance.yOffset);
                shockwaveParent = GameObject.Find("Void" + GameManager.Instance.arenaIndex.ToString()).transform;
            }
            sequence = DOTween.Sequence();
            sequence.Append(transform.DOMove(newPos , 1f).SetEase(Ease.Flash).OnComplete(() => {
                Instantiate(Resources.Load<GameObject>("Prefabs/VFX/Shockwave"), shockwaveParent.position, Quaternion.identity, shockwaveParent);
                CinemachineShake.Instance.ShakeCamera(6, 0.2f);
                ShootAOEPulse();
                circleExpandDuration = Random.Range(2, 7);
            }));
            sequence.Append(circleExpand.transform.DOScale(0.65f, circleExpandDuration));//expand circle
            sequence.Join(circleExpand.GetComponent<SpriteRenderer>().DOColor(new Color(circleExpand.GetComponent<SpriteRenderer>().color.r, circleExpand.GetComponent<SpriteRenderer>().color.g, circleExpand.GetComponent<SpriteRenderer>().color.b, 1f), circleExpandDuration));//colour circle
            sequence.Join(circleExpand.transform.DOLocalRotate(new Vector3(0,0, -360), circleExpandDuration, RotateMode.FastBeyond360).SetEase(Ease.OutSine));
            sequence.Append(transform.DOShakePosition(1f, new Vector3(0.5f, 0, 0), 5, 0).SetEase(Ease.Flash).OnComplete(() => {
                Instantiate(Resources.Load<GameObject>("Prefabs/VFX/Shockwave"), shockwaveParent.position, Quaternion.identity, shockwaveParent);
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

    private void OnDestroy() {
        sequence.Kill();
        
    }

    private void Update() {
        if (!GameManager.Instance.isGamePlaying){
            sequence.Pause();
        }
        else{
            sequence.Play();
        }
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
