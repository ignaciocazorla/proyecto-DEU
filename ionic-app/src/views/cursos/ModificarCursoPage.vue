<template>
     <ion-page>
        <ion-header>
            <ion-toolbar>
                <ion-buttons slot="start">
                    <ion-back-button default-href="/tabs/cursos"></ion-back-button>
                </ion-buttons>
                <ion-title tabindex="0">Modificar nombre del curso</ion-title>
            </ion-toolbar>
        </ion-header>
        <ion-content>
            <div class="form-container">
                <form v-on="curso" v-if="curso != undefined">
                    <ion-item>
                        <ion-label position="floating" for="nombreCurso">Nombre del curso:</ion-label>
                        <ion-input type="text" v-model="nombreCurso" :value="curso.nombre" />
                    </ion-item>
                <ion-button @click="updateCurso(curso, nombreCurso)">Modificar</ion-button>
                </form>
            </div>
        </ion-content>
    </ion-page>
</template>

<script>
import {  IonPage, IonHeader, IonToolbar, IonTitle, IonContent, IonInput, IonLabel, IonButton, IonButtons, IonBackButton, IonItem, } from '@ionic/vue';
import axios from 'axios';
import { loading } from '../overlay-views/loading.js';
import { alertDialog } from '../overlay-views/alertDialog.js';

export default {
    components: { IonPage, IonHeader, IonToolbar, IonTitle, IonContent, IonInput, IonLabel, IonButton, IonButtons, IonBackButton, IonItem, },
    methods:{
        updateCurso(curso, nuevoNombre){
            loading.showMsg("Modificando curso...");
            axios.put("/api/Cursos/" + curso.id,{
                "id": curso.id,
                "nombre": nuevoNombre
                }).then(() => {
                    loading.hide();
                    alertDialog.showAlertMsg("Curso modificado satisfactoriamente!");
                    curso.nombre = nuevoNombre;
                });
            }
    },
    mounted(){
        let curso = this.$store.getters["cursos/curso"](this.$route.params.id);
        if(curso == undefined){
            loading.showMsg("Cargando...");
            axios.get("/api/Cursos/" + this.$route.params.id)
            .then(resp => {
                console.log(resp);
                this.$store.commit("cursos/load",[resp.data]);
                loading.hide();
            })
            .catch(err => {
            console.log(err);
            loading.hide();
            });
        }
    },
    computed:{
        curso(){
            return this.$store.getters["cursos/curso"](this.$route.params.id);
        }
    }
}
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

ion-button{
    margin-top: 10%;
}

</style>