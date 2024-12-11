var functions = {
	 IsMobile: function () {
        return Module.SystemInfo.mobile;
    },
    AskAuth: function(){
       auth();
    },

     IsAuth: function(){
     if(player.getMode() === 'lite'){
         return false;
     }else{
         return true;
     }
    },

    SaveExtern: function(data){
        var dataStr = UTF8ToString(data);
        var myobj = JSON.parse(dataStr);  
        try{
        player.setData(myobj).then(() => {
                    console.log('data is set');
                }).catch(()=>{
                    console.log('data is not set');
                });
            }catch (err){
                console.error('my auth error: ', err.message);
            }
    },


    LoadExtern: function(){
        player.getData().then(_data =>{
            const myJSON = JSON.stringify(_data);          
            myGameI.SendMessage('YandexManager','FinishLoadFromSDK',myJSON);
        });
    },

    GetLang: function(){
        var lang=ysdk.environment.i18n.lang;
        var bufferSize = lengthBytesUTF8(lang)+1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(lang,buffer,bufferSize);
        return buffer;
    },

    CanReview: function(){
        ysdk.feedback.canReview()
            .then(({ value, reason }) => {

                console.log(value);
                console.log(reason);

                if(value){
                    myGameI.SendMessage('YandexManager','FinishCanReview','true');
                }else{
                    myGameI.SendMessage('YandexManager','FinishCanReview',reason);
                }

            });
    },

    TryAskReview: function(){
        ysdk.feedback.canReview()
            .then(({ value, reason }) => {

                console.log(value);
                console.log(reason);

                if (value) {
                    ysdk.feedback.requestReview()
                        .then(({ feedbackSent }) => {
                        if(feedbackSent){
                            myGameI.SendMessage('YandexManager','FinishAskReview','true');
                        }else{
                            myGameI.SendMessage('YandexManager','FinishAskReview','false');
                        }
                            console.log("fb sent");
                            console.log(feedbackSent);
                        })
                } else {
                    console.log(reason);
                }
            });     
    },

    Focus: function(){
        FocusGame();
    },

    UpdateLBExtern: function(score){
      if (ysdk !== null && lb !==null){
          //-----SET HARDCODE LB NAME------
          lb.setLeaderboardScore('LevelLeaderboard', score);
      }
        
    },

    GameReadyAPIReadyExtern: function(){
      console.log('GameReadyAPIReadyExtern');
      if (ysdk !== null && ysdk.features.LoadingAPI !== undefined && ysdk.features.LoadingAPI !== null){
          ysdk.features.LoadingAPI.ready();
          console.log("ready");
      }
        
    },

    //ads

    ShowRewardedAdExtern : function(){
    console.log('ShowRewardedAdExtern');
        if (ysdk !== null){
            try{
                ysdk.adv.showRewardedVideo({
                    callbacks: {
                        onOpen: () => {
                          console.log('Video ad open.');
                          isAdOpened=true;
                          myGameI.SendMessage('YandexManager','RewardedAdOpen');
                        },
                        onRewarded: () => {
                          console.log('Rewarded!');
                          myGameI.SendMessage('YandexManager','RewardedAdShown');
                        },
                        onClose: () => {
                          console.log('Video ad closed.');
                          isAdOpened=false;
                          myGameI.SendMessage('YandexManager','RewardedAdClosed');
                        },
                        onError: (e) => {
                          console.log('Error while open video ad:', e);
                          isAdOpened=false;
                        }
                    }
                });
            }catch (err) {
                console.error('CRASH Rewarded Video Ad Show: ', err.message);
            }
        }
    },

     ShowInterExtern : function(){
        showInter();
    },

     StartGetOtherGamesURL: function(){
       ysdk.features.GamesAPI.getAllGames().then(({games, developerURL}) => {
                myGameI.SendMessage('YandexManager','SetOtherGamesUrl',developerURL);
          }).catch(err => {
             console.log('error getting other games'+err)
          })
    },

    
};

mergeInto(LibraryManager.library, functions);