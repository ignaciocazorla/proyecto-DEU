<template>
     <ion-page>
        <ion-header>
            <ion-toolbar>
                <ion-buttons slot="start">
                    <ion-back-button default-href="/tabs/cursos"></ion-back-button>
                </ion-buttons>
                <ion-title tabindex="0">Nuevo recurso</ion-title>
            </ion-toolbar>
        </ion-header>
        <ion-content>
            <div class="ion-padding">
                <form >
                    <ion-item>
                        <ion-label position="floating" for="tituloRecurso">Titulo del recurso:</ion-label>
                        <ion-input type="text" v-model="tituloRecurso" />
                    </ion-item>
                    <ion-item>
                        <ion-label position="floating" for="textoRecurso">Texto del recurso:</ion-label>
                        <ion-textarea type="text" v-model="textoRecurso"></ion-textarea>
                    </ion-item>

                <ion-list>
                    <ion-list-header>Enlaces:</ion-list-header>
                    <ion-item v-if="enlaces.length == 0"> 
                        <ion-text>No hay enlaces cargados. Presione agregar para cargar enlaces.</ion-text>
                    </ion-item>
                    <ion-item v-else v-bind:key="enlace.nombre" v-for="enlace in enlaces">
                        <ion-label>{{enlace.nombre}}</ion-label>
                    </ion-item>
                    <ion-item>
                        <ion-button @click="showAgregarItemEnlace()">+Agregar enlace</ion-button>
                    </ion-item>
                    <div class="item-cargar-enlace" v-if="this.shouldShow">
                        <ion-item>
                            <ion-label position="floating">Ingrese nombre del enlace:</ion-label>
                            <ion-input v-model="nombre"></ion-input>
                        </ion-item>
                        <ion-item>
                            <ion-label position="floating">Ingrese el enlace:</ion-label>
                            <ion-input v-model="url"></ion-input>
                        </ion-item>
                        <ion-item>
                            <ion-button @click="agregarEnlace(nombre, url)">Agregar</ion-button>
                            <ion-button @click="showAgregarItemEnlace">Cancelar</ion-button>
                        </ion-item>
                    </div>
                </ion-list>

                <ion-button class="post-button" @click="postRecurso">Crear</ion-button>
                </form>
            </div>
        </ion-content>
    </ion-page>
</template>

<script>
import {  IonPage, IonHeader, IonToolbar, IonTitle, IonContent, IonButtons, IonButton, IonLabel, IonBackButton, IonInput, IonTextarea, IonListHeader, IonList, IonItem, IonText, } from '@ionic/vue';
import axios from 'axios';
import { loading } from '../overlay-views/loading.js';
import { alertDialog } from '../overlay-views/alertDialog.js';

export default({
  data() {
    return{
        tituloRecurso: '',
        textoRecurso: '',
        shouldShow: false,
        enlacesList: [],
        nombre: '',
        url: '',
    };
  },
  components: { IonPage, IonHeader, IonToolbar, IonTitle, IonContent, IonButtons, IonButton, IonLabel, IonBackButton, IonInput, IonTextarea, IonListHeader, IonList, IonItem, IonText, },

  methods: {
    postRecurso() {
        loading.showMsg('Creando recurso...');

        axios.post("/api/Recursos", {
            "titulo": this.tituloRecurso,
            "texto": this.textoRecurso,
            "IdCurso": this.$store.getters["cursos/cursoActual"],
            "enlaces": this.enlacesList,
        }).then(resp => {
            loading.hide();  
            alertDialog.showAlertMsg("Recurso creado con exito!");
            this.$store.dispatch('recursos/add', resp.data);
        })
        .catch(err => {
            loading.hide();
            alertDialog.showAlertMsg("No se pudo crear el recurso. Error: " + err.message);
        });
    },

    showAgregarItemEnlace(){
        this.nombre = '';
        this.url = '';
        let item = document.getElementsByClassName('item-cargar-enlace');
        this.shouldShow = !this.shouldShow
        item.display = this.shouldShow ? 'block' : 'none';
    },

    agregarEnlace(nombre, url){
        this.enlacesList.push({"nombre": nombre, "url": url});
        this.showAgregarItemEnlace();
    }
  },
  computed:{
    enlaces(){
        return this.enlacesList;
    }
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

.post-button{
    position: relative;
    margin-top: 10%;
    width: 100%;
}

/*ion-textarea{
    border-color: #403E39;
    border-width: thin;
    border-style: solid;
    border-radius: 3px;
        
}*/
</style>