<template>
    <tabs-base pageTitle="Registrarse">
        <form @submit.prevent="onSubmit(username,email,repeatEmail,password,repeatPassword)">
            <ion-label for="nombre de usuario">Nombre de usuario:</ion-label>
            <ion-input type="text" id="username" v-model="username" />
            <ion-label for="email">Email:</ion-label>
            <!-- <ion-input type="text" name="email" id="email" v-model="email" /> -->
            <ion-input type="text" id="email" v-model="email" />
            <ion-label for="email">Confirmar email:</ion-label>
            <ion-input type="text" id="email" v-model="repeatEmail" />
            <ion-label for="password">Contraseña:</ion-label>
            <ion-input type="password" id="password" v-model="password" />
            <ion-label for="password">Confirmar contraseña:</ion-label>
            <ion-input type="password" id="password" v-model="repeatPassword" />
            <ion-button type="submit">Terminar registro</ion-button>
        </form>
    </tabs-base>
</template>

<script>
import TabsBase from '../components/base/TabsBase.vue';
import { IonInput, IonLabel, IonButton } from '@ionic/vue';
import axios from 'axios';

export default {
    components: {
        TabsBase,
        IonInput,
        IonLabel,
        IonButton,
    },
    methods: {
    onSubmit(username,email,repeatEmail,password,repeatPassword) {
      console.log(username);
      console.log(email);
      console.log(repeatEmail);
      console.log(password);
      console.log(repeatPassword);
      if(email != repeatEmail ){
        console.log("Los email no coinciden!");
        return false;
      }
      if(password != repeatPassword ){
        console.log("Las contraseñas no coinciden!");
        return false;
      }
      this.postUser(username, email, password);
    },

    postUser(username, email, password){
        axios.post("/api/register", {
                "username": username,
                "email": email,
                "password": password,
        }).then(resp => {
            console.log(resp);
        })
    }
  },
}
</script>