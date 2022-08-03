<template>
     <ion-page>
        <ion-header>
            <ion-toolbar>
                <ion-buttons slot="start">
                    <ion-back-button default-href="/tabs/cursos"></ion-back-button>
                </ion-buttons>
                <ion-title tabindex="0">Nuevo curso</ion-title>
            </ion-toolbar>
        </ion-header>
        <ion-content>
            <div class="form-container">
                <form>
                <ion-label for="nombreCurso">Nombre del curso:</ion-label>
                <ion-input type="text" v-model="nombreCurso" />
                <ion-button @click="postCurso">Crear</ion-button>
                </form>
            </div>
        </ion-content>
    </ion-page>
</template>

<script>
import {  IonPage, IonHeader, IonToolbar, IonTitle, IonContent, IonButtons, IonButton, IonLabel, IonBackButton, IonInput, } from '@ionic/vue';
import axios from 'axios';
import { loading } from '../overlay-views/loading.js';
import { alertDialog } from '../overlay-views/alertDialog.js';

export default({
  data() {
    return{
        nombreCurso: ''
    };
  },
  components: { IonPage, IonHeader, IonToolbar, IonTitle, IonContent, IonButtons, IonButton, IonLabel, IonBackButton, IonInput },

  methods: {
    postCurso() {
        //console.log(this.nombreCurso);
        /*const loading = await loadingController.create({
          message: 'Creando curso...',
          //duration: 3000
        });
        loading.present();*/
        loading.showMsg("Creando curso...");

        axios.post("/api/Cursos", {
            "nombre": this.nombreCurso,
            "idDocente": this.$store.getters["usuario/usuario"].id,
        }).then(resp => {
            loading.hide();  
            alertDialog.showAlertMsg("Curso creado con exito!");
            console.log(resp.data);
            this.$store.dispatch('cursos/add', resp.data);
        })
        .catch(err => {
            loading.hide();
            alertDialog.showAlertMsg("No se pudo crear el curso. Error: " + err.message);
        });
    },
    /*async presentAlert(msg) {
      const alert = await alertController.create({
        //header: 'Alert',
        //subHeader: 'Important message',
        message: msg,
        buttons: ['OK']
      });

      await alert.present();
    },*/
  },
});
</script>

<style scoped>
.form-container {
  text-align: center;
  position: absolute;
  left: 0;
  right: 0;
  top: 50%;
  transform: translateY(-50%);
}
</style>