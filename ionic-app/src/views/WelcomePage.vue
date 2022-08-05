<template>
    <ion-page>
        <ion-header>
        </ion-header>
        <ion-content fullscreen="true">
            
            <div class="container ion-padding ion-align-self-center">
                <h1> NombreDeLaApp </h1>
                <p> NombreDeLaApp te permite gestionar y acceder tus materiales educativos. 
                    Además es una herramienta que permite enseñar y aprender de una manera distinta.
                </p>
                <ion-button router-link="login"> Comenzar </ion-button>
            </div>
        </ion-content>
    </ion-page>
</template>

<script>
import { IonPage, IonHeader, IonContent, IonButton } from "@ionic/vue";

export default {
    components:{
        IonPage,
        IonHeader,
        IonContent,
        IonButton
    }    
}
</script>

<style scoped>
#container {
  top: 500px;
  transform: translate(-50%, -50%);
  /*text-align: center;
  position: absolute;
  left: 50%;
  right: 50%;*/
}

</style>