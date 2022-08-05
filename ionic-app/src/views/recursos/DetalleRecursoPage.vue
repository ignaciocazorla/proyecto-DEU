<template>
    <ion-page>
        <ion-header>
            <ion-toolbar>
                <ion-buttons slot="start">
                    <ion-back-button default-href="/tabs/cursos"></ion-back-button>
                </ion-buttons>
                <ion-title tabindex="0">Detalle del recurso</ion-title>
            </ion-toolbar>
        </ion-header>
        <ion-content>
            <ion-card v-if="recurso == null">
            </ion-card>
            <ion-card v-else>
                <ion-card-header v-model="recurso">
                <ion-card-title> {{ recurso.titulo }} </ion-card-title>
                </ion-card-header>
                <ion-card-content class="textarea-spaces">
                    {{ recurso.texto }}
                    <div v-if="recurso.enlaces.length != 0">
                        <ion-list>
                            <ion-list-header>Enlaces</ion-list-header>
                            <ion-item v-bind:key="enlace.nombre" v-for="enlace in recurso.enlaces">
                                <a :href="enlace.url" target="_blank">{{enlace.nombre}}</a>
                            </ion-item>
                        </ion-list>
                    </div>
                    <ion-item>
                        <ion-button v-if="(this.$store.getters['usuario/usuario'].rol == 'Docente')" size="small" fill="outline" :router-link="`/tabs/cursos/recursos/update/${recurso.id}`" slot="end">Modificar</ion-button>
                    </ion-item>
                </ion-card-content>
            </ion-card>
        </ion-content>
    </ion-page>
</template>

<script>
import {  IonPage, IonHeader, IonToolbar, IonTitle, IonContent, IonButtons, IonBackButton, IonCard, IonCardHeader, IonCardContent, IonCardTitle, IonList, IonListHeader, IonItem, IonButton, } from '@ionic/vue';
import axios from 'axios';
import { loading } from '../overlay-views/loading.js';
//import { Browser } from '@capacitor/browser'; //Para aplicacion nativa

export default {
    data(){
        return{
            recursoActual: null
        }
    },
    components:{  IonPage, IonHeader, IonToolbar, IonTitle, IonContent, IonButtons, IonBackButton, IonCard, IonCardHeader, IonCardContent, IonCardTitle, IonList, IonListHeader, IonItem, IonButton, },
    mounted(){
        this.recursoActual = this.$store.getters["recursos/recurso"](this.$route.params.id);
        if(this.recursoActual == null){
            loading.showMsg("Cargando...");
            axios.get("/api/Recursos/" + this.$route.params.id)
            .then(resp => {
                this.recursoActual = resp.data[0];
                loading.hide();
            })
            .catch(err => {
                console.log(err);
                loading.hide();
            });
        }
    },
    methods:{
        /*async openUrl(url){
            await Browser.open({ url: url });
        }*/
    },
    computed:{
        recurso(){
            return this.recursoActual;
        }
    }
}
</script>

<style scoped>
.textarea-spaces{
    white-space: pre-wrap;
}
</style>