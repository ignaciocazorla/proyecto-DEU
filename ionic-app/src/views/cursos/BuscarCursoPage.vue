<template>
    <ion-page>
        <ion-header>
            <ion-toolbar>
              <ion-buttons slot="start">
                    <ion-back-button default-href="/tabs/cursos"></ion-back-button>
                </ion-buttons>
                <ion-title tabindex="0">Buscar cursos</ion-title>
            </ion-toolbar>
        </ion-header>
        <ion-content>
            <ion-searchbar @ionChange="handleInput($event)"></ion-searchbar>

            <ion-list>
                <ion-item v-bind:key="curso.id" v-for="curso in lista">
                  <ion-text>{{curso.nombre}}</ion-text>
                  <ion-button @click="inscribir(curso.id)" slot="end">Inscribirse </ion-button>
                </ion-item>
            </ion-list>

        </ion-content>
    </ion-page>
</template>

<script>
import {IonPage, IonHeader, IonTitle, IonToolbar, IonContent, IonSearchbar, IonList, IonItem, IonText, IonButtons, IonBackButton, IonButton,} from '@ionic/vue';
import axios from 'axios';
import { loading } from '../overlay-views/loading.js';
import { alertDialog } from '../overlay-views/alertDialog.js';

export default {
    components: {IonPage, IonHeader, IonTitle, IonToolbar, IonContent, IonSearchbar, IonList, IonItem, IonText, IonButtons, IonBackButton, IonButton, },
    mounted() {    
      loading.showMsg('Cargando cursos...');

      axios.get("/api/Cursos/")
      .then(resp => {
          this.$store.dispatch("busquedaCursos/load",resp.data);
          loading.hide();
      })
      .catch(err => {
        console.log(err);
        loading.hide();
      });

      let cursos = this.$store.getters["cursos/cursos"];

      let config = {
          headers: {
            "authorization": "Bearer " + localStorage.getItem("token"),
        }
      }

      if (cursos.length == 0){
        axios.get("/api/Cursos/usuario", config)
          .then(resp => {
              this.$store.dispatch("cursos/load",resp.data);
          })
          .catch(err => {
            console.log(err);
          });
      }
    
  },
  methods:{
    inscribir(cursoId){
        loading.showMsg('Inscribiendose al curso...');

        axios.post("/api/Cursos/inscribir", {
            "cursoId": cursoId,
            "estudianteId": this.$store.getters["usuario/usuario"].id
        }).then(() => {
            //this.$store.dispatch("cursos/add",this.$store.getters["cursos/curso", cursoId]);
            loading.hide();
            alertDialog.showAlertMsg("Inscripción realizada correctamente!");
        })
        .catch(err => {
            console.log(err);
            loading.hide();
        });
    },
    handleInput(event) {
        const items = Array.from(document.querySelector('ion-list').children);      
        const query = event.target.value.toLowerCase();
        requestAnimationFrame(() => {
          items.forEach((item) => {
            const shouldShow = item.textContent.toLowerCase().indexOf(query) > -1;
            item.style.display = shouldShow ? 'block' : 'none';
          });
        });
      }
  },
    computed: {
    lista() {
       return this.$store.getters["busquedaCursos/busquedaCursos"].filter(
        elem => !this.$store.getters["cursos/cursos"].some(curso => curso.id == elem.id));
    }
  },
}
</script>