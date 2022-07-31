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
      </ion-list>
    </ion-content>
  </ion-page>
</template>

<script>
import { defineComponent } from 'vue';
import { IonPage, IonHeader, IonToolbar, IonTitle, IonContent, IonToggle, IonList, IonListHeader, IonItem, IonLabel,} from '@ionic/vue';

export default defineComponent({

  name: 'Tab3Page',
  data() {
    return{
      darkMode: false,
    }
  },
  components: { IonHeader, IonToolbar, IonTitle, IonContent, IonPage, IonToggle, IonList, IonListHeader, IonItem, IonLabel,},
  methods:{
    cambiarTema(){
      this.darkMode = !this.darkMode;
      const prefersDark = window.matchMedia('(prefers-color-scheme: dark)');
      console.log(prefersDark.matches);
      this.darkMode = prefersDark.matches;
      
      document.body.classList.toggle('dark');
    }
  },
  mounted() {
    const prefersDark = window.matchMedia('(prefers-color-scheme: dark)');
    this.darkMode = prefersDark.matches;
    //prefersDark.addEventListener(() => {cambiarTema()})
    if(this.darkMode){
      document.body.classList.toggle('dark');
    }
  }
});
</script>
