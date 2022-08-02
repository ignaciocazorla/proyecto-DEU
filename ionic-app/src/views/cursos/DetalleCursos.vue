<template>
    <ion-page>
        <ion-header>
            <ion-toolbar>
                <ion-buttons slot="start">
                    <ion-back-button default-href="/tabs/cursos"></ion-back-button>
                </ion-buttons>
                <ion-title tabindex="0">Detalle de curso</ion-title>
            </ion-toolbar>
        </ion-header>
        <ion-content>
            <ion-list tabindex="0">
              <ion-card v-if="(this.$store.getters['usuario/usuario'].rol == 'Docente')" @click="storeIdCurso()" button router-link="recursos/add">
                 <ion-card-header>
                  <ion-card-title> + Agregar nuevo recurso</ion-card-title>
                </ion-card-header>
              </ion-card>
              
              <h1 class="empty-list-msg" v-if="recursos.length == 0"> No hay elementos para mostrar</h1>

              <div v-bind:key="recurso.id" v-for="recurso in recursos">
                <ion-card button :router-link="`recursos/${recurso.id}`">
                  <ion-card-header>
                    <ion-card-title> {{ recurso.titulo }} </ion-card-title>
                  </ion-card-header>
                </ion-card>
                <ion-button v-if="(this.$store.getters['usuario/usuario'].rol == 'Docente')" size="small" fill="outline" @click="deleteRecurso(recurso.id)">Eliminar</ion-button>
                <ion-button v-if="(this.$store.getters['usuario/usuario'].rol == 'Docente')" size="small" fill="outline">Modificar</ion-button>
              </div>
            </ion-list>
        </ion-content>
    </ion-page>
</template>

<script>
import { defineComponent } from 'vue';
import {  IonPage, IonHeader, IonToolbar, IonTitle, IonContent, IonButtons, IonButton, IonBackButton, IonList, IonCard, IonCardHeader, /*IonCardContent,*/ IonCardTitle, } from '@ionic/vue';
import axios from 'axios';
import { loading } from '../overlay-views/loading.js';
import { alertDialog } from '../overlay-views/alertDialog.js';

export default defineComponent({
  name: 'DetalleCursos',
  components: { IonPage, IonHeader, IonToolbar, IonTitle, IonContent, IonButtons, IonButton, IonBackButton, IonList, IonCard, IonCardHeader, /*IonCardContent,*/ IonCardTitle, },
  data () {
    return {
        cursoId: this.$route.params.id,
    };
  },
  methods:{
    storeIdCurso(){
      this.$store.commit("cursos/loadCursoActual", this.cursoId);
    },

    deleteRecurso(id){
      alertDialog.showAlertMsgWithButtons("Esta acción no se puede deshacer. ¿Seguro que quiere eliminar el recurso?",
        [
          {
            text: 'Eliminar',
            role: 'delete',
            handler: () => { this.handlerMessage = this.delete(id); }
          },
          {
            text: 'Cancelar',
            role: 'cancel',
          }
        ]
      )
    },

    async delete(id){

      loading.showMsg('Eliminando recurso...');

      axios.delete("/api/Recursos/" + id)
      .then(() => {
        loading.hide();  
        alertDialog.showAlertMsg("Recurso eliminado!");
        this.$store.dispatch('recursos/delete', id);
      })
      .catch(err => {
            loading.hide();
            alertDialog.showAlertMsg("No se pudo eliminar el recurso. Error: " + err.message);
      });
    },

  },
  mounted() {
    loading.showMsg("Cargando recursos...");
    axios.get("/api/Recursos/Curso/" + this.cursoId)
    .then(resp => {
        this.$store.dispatch("recursos/load",resp.data);
        loading.hide();
    })
    .catch(err => {
      console.log(err);
      loading.hide();
    })
  },
  computed: {
    recursos() {
      return this.$store.getters["recursos/recursos"]
    },
  }
});
</script>