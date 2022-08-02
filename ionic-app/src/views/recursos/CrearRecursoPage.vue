<template>
     <ion-page>
        <ion-header>
            <ion-toolbar>
                <ion-buttons slot="start">
                    <ion-back-button default-href="recursos"></ion-back-button>
                </ion-buttons>
                <ion-title tabindex="0">Nuevo recurso</ion-title>
            </ion-toolbar>
        </ion-header>
        <ion-content>
            <div class="form-container">
                <form>
                <ion-label for="tituloRecurso">Titulo del recurso:</ion-label>
                <ion-input type="text" v-model="tituloRecurso" />
                <ion-label for="textoRecurso">Texto del recurso:</ion-label>
                <ion-item>
                    <ion-textarea type="text" v-model="textoRecurso"></ion-textarea>
                </ion-item>
                <ion-button @click="postRecurso">Crear</ion-button>
                </form>
            </div>
        </ion-content>
    </ion-page>
</template>

<script>
import {  IonPage, IonHeader, IonToolbar, IonTitle, IonContent, IonButtons, IonButton, IonLabel, IonBackButton, IonInput, IonTextarea,} from '@ionic/vue';
import axios from 'axios';
import { loading } from '../overlay-views/loading.js';
import { alertDialog } from '../overlay-views/alertDialog.js';

export default({
  data() {
    return{
        tituloRecurso: '',
        textoRecurso: '',
    };
  },
  components: { IonPage, IonHeader, IonToolbar, IonTitle, IonContent, IonButtons, IonButton, IonLabel, IonBackButton, IonInput, IonTextarea, },

  methods: {
    postRecurso() {
        loading.showMsg('Creando recurso...');

        console.log(this.$store.getters["cursos/cursoActual"])

        axios.post("/api/Recursos", {
            "titulo": this.tituloRecurso,
            "texto": this.textoRecurso,
            "IdCurso": this.$store.getters["cursos/cursoActual"],
        }).then(resp => {
            loading.hide();  
            alertDialog.showAlertMsg("Recurso creado con exito!");
            console.log(resp.data);
            this.$store.dispatch('recursos/add', resp.data);
        })
        .catch(err => {
            loading.hide();
            alertDialog.showAlertMsg("No se pudo crear el recurso. Error: " + err.message);
        });
    },
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