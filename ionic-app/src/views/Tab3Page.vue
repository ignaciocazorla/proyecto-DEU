<template>
  <ion-page>
    <ion-header>
      <ion-toolbar>
        <ion-title>Ajustes</ion-title>
      </ion-toolbar>
    </ion-header>
    <ion-content :fullscreen="true">
      <ion-header collapse="condense">
        <ion-toolbar>
          <ion-title size="large">Ajustes</ion-title>
        </ion-toolbar>
      </ion-header>

      <ion-list>
        <ion-list-header>
          <ion-label>Apariencia</ion-label>
        </ion-list-header>
        <ion-item>
          <ion-label> Activar/desactivar modo oscuro</ion-label>
          <ion-toggle :checked="darkMode" @ionChange="cambiarTema()" ></ion-toggle>
        </ion-item>

         <ion-list-header>
          <ion-label>Cuenta</ion-label>
        </ion-list-header>
        <ion-item>
          <ion-button @click="logout">Cerrar sesión</ion-button>
        </ion-item>
      </ion-list>
    </ion-content>
  </ion-page>
</template>

<script>
import { defineComponent } from 'vue';
import { IonPage, IonHeader, IonToolbar, IonTitle, IonContent, IonToggle, IonList, IonListHeader, IonItem, IonLabel, IonButton,} from '@ionic/vue';

export default defineComponent({

  name: 'Tab3Page',
  data() {
    return{
      darkMode: false,
    }
  },
  components: { IonHeader, IonToolbar, IonTitle, IonContent, IonPage, IonToggle, IonList, IonListHeader, IonItem, IonLabel, IonButton,},
  methods:{
    cambiarTema(){
      this.darkMode = !this.darkMode;     
      document.body.classList.toggle('dark');
    },

    logout(){
      localStorage.clear();
      this.$router.replace({ path: '/login' })
    }
  },
  mounted() {
    //https://developer.mozilla.org/es/docs/Web/API/MediaQueryList/addListener
    const prefersDark = window.matchMedia('(prefers-color-scheme: dark)');
    //this.darkMode = prefersDark.matches;
    if(prefersDark.matches){
      this.cambiarTema();
    }
  }
});
</script>
