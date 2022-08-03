<template>
    <tabs-base page-title="Mis cursos">
        <ion-list>
          <ion-item v-if="(this.$store.getters['usuario/usuario'].rol == 'Docente')" router-link="cursos/add" >
            <ion-button class="boton-agregar-item">+ Nuevo curso </ion-button>
          </ion-item>

          <ion-item v-if="(this.$store.getters['usuario/usuario'].rol == 'Estudiante')" router-link="cursos/search" >
            <ion-button class="boton-agregar-item">+ Inscripcion a cursos </ion-button>
          </ion-item>

          <ion-text>
            <p class="empty-list-msg" v-if="cursos.length == 0"> No hay elementos para mostrar.</p>
          </ion-text>

          <ion-item ref="listItem" v-bind:key="curso.id" v-for="curso in cursos"> <!--Agregar palabra button en el item para que se tome como boton-->
            <ion-button tabindex="0" :router-link="`cursos/${curso.id}`" key="">
              <strong>{{curso.nombre}}</strong> <br>
              <!-- <p>Cant. de estudiantes: {{curso.idEstudiates}}</p> -->
            </ion-button>
            <!-- <ion-button button id="click-trigger" slot="end"> -->
            <ion-button  v-if="(this.$store.getters['usuario/usuario'].rol == 'Docente')" button @click="openPopover($event, curso.id)" slot="end">
              <!--<ion-icon slot="icon-only" :icon="ellipsisVertical"></ion-icon> -->
              Más
            </ion-button>

            <ion-popover side="left" alignment="start"
            :is-open="popoverOpen"
            :event="event"
            @didDismiss="popoverOpen = false">
              <ion-item :id="`${curso.id}`" button @click="deleteCurso(this.popoverCursoId)">Eliminar curso</ion-item>
              <ion-item button @click="closePopover()" :router-link="`cursos/update/${this.popoverCursoId}`">Modificar nombre</ion-item>
          </ion-popover>
          </ion-item>
        </ion-list>
    </tabs-base>
</template>

<script>
import { IonList, IonItem, /*IonLabel,*/ IonButton, /*IonIcon,*/ IonPopover, IonText} from '@ionic/vue';
import TabsBase from "../../components/base/TabsBase.vue";
//import { chevronForwardOutline, ellipsisVertical } from 'ionicons/icons';
import axios from 'axios';
import { loading } from '../overlay-views/loading.js';
import { alertDialog } from '../overlay-views/alertDialog.js';

export default({
  name: 'ListadoCursos',
  components: {
    IonList,
    IonItem,
    /*IonLabel,*/
    TabsBase,
    IonButton,
    IonPopover,
    //IonIcon,
    IonText,
  },
  data() {
    return {
      popoverOpen: false,
      event: null,
      popoverCursoId: null
    }
  },
  async mounted() {

    let config = {
        headers: {
          "authorization": "Bearer " + localStorage.getItem("token"),
      }
    }

    //console.log(this.$store.getters["usuario/usuario"])
    
    loading.showMsg('Cargando cursos...');

    //axios.get("/api/Cursos/Docente/" + this.$store.getters["usuario/usuario"].id)
    axios.get("/api/Cursos/usuario/", config)
    .then(resp => {
        console.log(resp);
        this.$store.commit("cursos/load",resp.data);
        loading.hide();
    })
    .catch(err => {
      console.log(err);
      loading.hide();
    });
    
  },
  methods:{
    openPopover(e, id) {
        this.event = e;
        this.popoverOpen = true;
        this.popoverCursoId = id;
    },

    deleteCurso(id){
      this.popoverOpen = false;
      alertDialog.showAlertMsgWithButtons("Esta acción no se puede deshacer. ¿Seguro que quiere eliminar el curso?",
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

      loading.showMsg('Eliminando curso...');

      axios.delete("/api/Cursos/" + id)
      .then(() => {
        loading.hide();  
        alertDialog.showAlertMsg("Curso eliminado!");
        this.$store.dispatch('cursos/delete', id);
      })
      .catch(err => {
            loading.hide();
            alertDialog.showAlertMsg("No se pudo eliminar el curso. Error: " + err.message);
      });
    },

    closePopover(){
      this.popoverOpen = false;
    },

  },
  computed: {
    cursos() {
      return this.$store.getters["cursos/cursos"];
    },
  },
  setup(){
      return{
        //chevronForwardOutline,
        //ellipsisVertical
      }
    }
});
</script>

<style scoped>
.boton-agregar-item{
  background-color: lightseagreen;
  color: black;
}

.empty-list-msg{
  text-align: center;
  position: relative;
  left: 0;
  right: 0;
}

</style>