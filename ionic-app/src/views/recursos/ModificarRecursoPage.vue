<template>
      <ion-page>
        <ion-header>
            <ion-toolbar>
                <ion-buttons slot="start">
                    <ion-back-button default-href="/tabs/cursos"></ion-back-button>
                </ion-buttons>
                <ion-title tabindex="0">Modificar recurso</ion-title>
            </ion-toolbar>
        </ion-header>
        <ion-content>
            <div v-if="recurso != null" class="ion-padding">
                <form>
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
                        <ion-button @click="guardarElementoAQuitar(enlace.id)">Eliminar</ion-button>
                    </ion-item>
                    <ion-item>
                        <ion-button @click="showAgregarItemEnlace()">+Agregar enlace</ion-button>
                    </ion-item>
                    <div class="item-cargar-enlace" v-if="this.shouldShow">
                        <ion-item>
                            <ion-label position="floating">Ingrese nombre del enlace:</ion-label>
                            <ion-input v-model="nombre" ></ion-input>
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

                <ion-button class="post-button" @click="updateRecurso">Modificar</ion-button>
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
        recursoActual: null,
        tituloRecurso: '',
        textoRecurso: '',
        shouldShow: false,
        enlacesList: [],
        nombre: '',
        url: '',
        agregar: [],
        quitar: [],
    };
  },
  components: { IonPage, IonHeader, IonToolbar, IonTitle, IonContent, IonButtons, IonButton, IonLabel, IonBackButton, IonInput, IonTextarea, IonListHeader, IonList, IonItem, IonText, },
  mounted(){
        this.recursoActual = this.$store.getters["recursos/recurso"](this.$route.params.id);

        if(this.recursoActual != null){
            this.inicializarRecurso();
        }else{
            loading.showMsg("Cargando...");
            axios.get("/api/Recursos/" + this.$route.params.id)
            .then(resp => {
                this.recursoActual = resp.data[0];
                this.inicializarRecurso();
                loading.hide();
            })
            .catch(err => {
                console.log(err);
                loading.hide();
            });
        }
  },
  methods:{

    inicializarRecurso(){
        this.tituloRecurso = this.recursoActual.titulo;
        this.textoRecurso = this.recursoActual.texto;
        this.enlacesList = this.recursoActual.enlaces;
    },

    updateRecurso(){
        loading.showMsg('Modificando recurso...');

        axios.put("/api/Recursos/" + this.$route.params.id, {
            "titulo": this.tituloRecurso,
            "texto": this.textoRecurso,
            "enlaces": this.agregar,
            "enlacesBaja": this.quitar,
        }).then(() => {
            loading.hide();  
            alertDialog.showAlertMsg("Recurso modificado con éxito!");
            this.$store.dispatch('recursos/add', this.recursoActual);
        })
        .catch(err => {
            loading.hide();
            alertDialog.showAlertMsg("No se pudo modificar el recurso. Error: " + err.message);
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
        var r = new RegExp(/^(ftp|http|https):\/\/[^ "]+$/);
        console.log(r.test(url));
        this.agregar.push({"nombre": nombre, "url": url});
        this.enlacesList.push({"nombre": nombre, "url": url});
        this.showAgregarItemEnlace();
    },

    guardarElementoAQuitar(id){
        this.quitar.push(id);
        this.enlacesList = this.enlacesList.filter(elem => elem.id != id);
    }, 

    /*validar(event, nombre){
        console.log(event);
        //console.log(event.srcElement.parentElement.parentElement.setAttribute('class','ion-invalid'));
        console.log(nombre);
    },*/
  },
  computed:{
    enlaces(){
        return this.enlacesList;
    },
    recurso(){
        return this.recursoActual;
    }
  }

})
</script>

<style scoped>

.post-button{
    width: 100%;
    margin-top: 10%;
}

</style>