﻿using System;
using Firebase.Auth;
using Firebase.Firestore;
using UnityEngine ;
using TMPro ;

public class Player : MonoBehaviour {
   /*[old code]
   [SerializeField] SpriteRenderer playerImage ;
   */
   [SerializeField] GameObject[] skins ;
   [SerializeField] TMP_Text playerNameText ;

   public float speed = 1f ;

   Rigidbody2D rb ;
   bool isMoving = false ;
   float x, y ;

   void Start()
   {
      if (!FirebaseManager.Instance.initialized)
      {
         // Initialize Firebase
         FirebaseManager.Instance.OnFirebaseInitialized += FirebaseManager_OnInitialized_Handler;
         FirebaseManager.Instance.InitializeFirebase();
         return;
      }

      rb = GetComponent<Rigidbody2D>();
      ChangePlayerSkin();
   }

   private void OnDisable()
   {
      FirebaseManager.Instance.OnFirebaseInitialized -= FirebaseManager_OnInitialized_Handler;
   }
   
   private void FirebaseManager_OnInitialized_Handler(FirebaseAuth arg1, FirebaseFirestore arg2)
   {
      rb = GetComponent<Rigidbody2D>();
      ChangePlayerSkin();
   }

   void ChangePlayerSkin () {
      Character character = GameDataManager.GetSelectedCharacter () ;
      if (character.image != null) {
      	 /*[old code]
         playerImage.sprite = character.image ;
         */

         // Get selected character's index:
         int selectedSkin = GameDataManager.GetSelectedCharacterIndex () ;

         // show selected skin's gameobject:
         skins [ selectedSkin ].SetActive (true) ;

         // hide other skins (except selectedSkin) :
         for (int i = 0; i < skins.Length; i++)
            if (i != selectedSkin)
               skins [ i ].SetActive (false) ;


         playerNameText.text = character.name ;
      }
   }

   void Update () {
      #if UNITY_EDITOR
      if (!Input.GetKey (KeyCode.M)) {
         if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.DownArrow)) {
            x = Input.GetAxis ("Horizontal") ;
            y = Input.GetAxis ("Vertical") ;

         } else
            x = y = 0f ;
      }
      #endif

      isMoving = (x != 0f || y != 0f) ;
   }

   public void MoveHorizontally (float value) {
      x = value ;
   }

   public void MoveVertically (float value) {
      y = value ;
   }

   void FixedUpdate () {
      if (isMoving) {
         rb.position += new Vector2 (x, y) * speed * Time.fixedDeltaTime ;
      }
   }

   private void OnTriggerEnter2D(Collider2D col)
   {
      var tag = col.tag;

      if (tag.Equals ("coin")) {
         //Add Coins 
         GameDataManager.AddCoins(100) ;

         // Cheating (while moving hold key "C" to get extra coins) 
#if UNITY_EDITOR
         if (Input.GetKey (KeyCode.C))
            GameDataManager.AddCoins (179) ;
#endif

         GameSharedUI.Instance.UpdateCoinsUIText () ;

         Destroy (col.gameObject) ;
      }
   }
}
