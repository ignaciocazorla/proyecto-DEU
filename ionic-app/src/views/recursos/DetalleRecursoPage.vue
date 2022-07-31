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
            <ion-card v-if="recurso != undefined">
                <ion-card-header v-model="recurso">
                <ion-card-title> {{ recurso.titulo }} </ion-card-title>
                </ion-card-header>
                <ion-card-content>
                    {{ recurso.texto }}
                </ion-card-content>
            </ion-card>
        </ion-content>
    </ion-page>
</template>

<script>
import {  IonPage, IonHeader, IonToolbar, IonTitle, IonContent, IonButtons, IonBackButton, IonCard, IonCardHeader, IonCardContent, IonCardTitle, } from '@ionic/vue';
import axios from 'axios';
import { loading } from '../overlay-views/loading.js';

export default {
    components:{  IonPage, IonHeader, IonToolbar, IonTitle, IonContent, IonButtons, IonBackButton, IonCard, IonCardHeader, IonCardContent, IonCardTitle, },
    mounted(){
        let recurso = this.$store.getters["recursos/recurso"](this.$route.params.id);
        if(recurso == undefined){
            loading.showMsg("Cargando...");
            axios.get("/api/Recursos/" + this.$route.params.id)
            .then(resp => {
                console.log(resp);
                this.$store.commit("recursos/load",[resp.data]);
                loading.hide();
            })
            .catch(err => {
                console.log(err);
                loading.hide();
            });
        }
    },
    computed:{
        recurso(){
            return this.$store.getters["recursos/recurso"](this.$route.params.id);
        }
    }
}
</script>