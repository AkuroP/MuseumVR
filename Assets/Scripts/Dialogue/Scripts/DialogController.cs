using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using static UnityEngine.EventSystems.EventTrigger;

public class DialogController : MonoBehaviour
{

    //public Text txtSentence;

    public int _idCurrentSentence = 0;

    public DialogConfig _dialog;
    public AudioSource _audioSource;
    [SerializeField] InputActionReference skipDialogInput;
    private InputAction nextDialog;

    private Entity entityConcerned;

    private Player player;

    [SerializeField]
    private Material normalMat;
    [SerializeField]
    private Material dimensionMat;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    private void OnEnable()
    {
        //if(_dialog == null)return;
        //PlayDialog(_dialog);
        nextDialog = skipDialogInput;
        nextDialog.Enable();
        nextDialog.performed += NextDialog;
    }

    private void OnDisable()
    {
        nextDialog.Disable();
    }


    public void PlayDialog(Entity entity)
    {
        _dialog = entity.EntityDialog[entity.CurrentDialogAdvancement];
        if (_dialog.onDialog) return;
        entityConcerned = entity;
        
        //stop time
        //Time.timeScale = 1f;
    
        //activate dialog
        gameObject.SetActive(true);

        _dialog.firstCharTxt.transform.parent.gameObject.SetActive(true);
        /*txtNameLeft.text = actualDialog.nameLeft;
        imgSpriteLeft.sprite = actualDialog.spriteLeft;

        txtNameRight.text = actualDialog.nameRight;
        imgSpriteRight.sprite = actualDialog.spriteRight;*/
        _dialog.onDialog = true;
        RefreshBox();
    }

    private void RefreshBox()
    {
        DialogConfig.SentenceConfig sentence = _dialog.sentenceConfig[_idCurrentSentence];

        /*switch(sentence.position)
        {
            case DialogConfig.SentenceConfig.POSITION.LEFT :
                txtNameLeft.color = Color.white;
                txtNameRight.color = Color.clear;

                imgSpriteLeft.color = Color.white;
                imgSpriteRight.color = Color.gray;
            break;

            case DialogConfig.SentenceConfig.POSITION.RIGHT :
                txtNameLeft.color = Color.clear;
                txtNameRight.color = Color.white;

                imgSpriteLeft.color = Color.gray;
                imgSpriteRight.color = Color.white;
            break;
        }*/

        switch(sentence.whoTalk)
        {
            case DialogConfig.SentenceConfig.CHARACTER.FIRSTCHAR:
                _dialog.firstCharTxt.transform.parent.gameObject.SetActive(true);
                
                _dialog.firstCharTxt.text = sentence.sentence;
                if(player.IsInDimension)_dialog.firstCharTxt.transform.parent.GetComponent<Renderer>().material = dimensionMat;
                else _dialog.firstCharTxt.transform.parent.GetComponent<Renderer>().material = normalMat;
                //Debug.Log(_dialog.firstCharTxt.transform.parent.name);

                if (_dialog.secondCharTxt != null) _dialog.secondCharTxt.transform.parent.gameObject.SetActive(false);
            break;
            case DialogConfig.SentenceConfig.CHARACTER.SECONDCHAR:
                _dialog.secondCharTxt.transform.parent.gameObject.SetActive(true);

                _dialog.secondCharTxt.text = sentence.sentence;
                if (player.IsInDimension) _dialog.secondCharTxt.transform.parent.GetComponent<Renderer>().material = normalMat;
                else _dialog.secondCharTxt.transform.parent.GetComponent<Renderer>().material = dimensionMat;
                

                if(_dialog.firstCharTxt != null) _dialog.firstCharTxt.transform.parent.gameObject.SetActive(false);
                
                break;
        }

        //stop actual audio
        if(_audioSource == null)return;
        _audioSource.Stop();
        
        //set next audio
        _audioSource.clip = sentence.sfx;
        
        //play next audio
        _audioSource.Play();
    }

    public void NextSentence()
    {
        if(_dialog == null)return;
        _idCurrentSentence++;

        if (_idCurrentSentence < _dialog.sentenceConfig.Count) RefreshBox();
        else
        {
            if (_dialog.sentenceConfig[_dialog.sentenceConfig.Count - 1].increment)
            {
                entityConcerned.CurrentDialogAdvancement += 1;
                Debug.Log("AJOUTED ZBI");
            }
            CloseDialog();
        }

    }

    public void CloseDialog()
    {
        //resume time
        //Time.timeScale = 0f;
        //reset dialog var and close gameobject
        _dialog.onDialog = false;
        //this.gameObject.SetActive(false);

        _dialog.firstCharTxt.transform.parent.gameObject.SetActive(false);
        if(_dialog.secondCharTxt != null) _dialog.secondCharTxt.transform.parent.gameObject.SetActive(false);
        _idCurrentSentence = 0;
        entityConcerned = null;
        _dialog = null;
    }

    public void NextDialog(InputAction.CallbackContext ctx)
    {
        if(_dialog == null || !_dialog.onDialog) return;
        NextSentence();

    }

}
