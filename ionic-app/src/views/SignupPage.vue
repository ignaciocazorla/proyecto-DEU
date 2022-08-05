<template>
    <tabs-base pageTitle="Registrarse">
        <form class="ion-padding" @submit.prevent="onSubmit(username,email,repeatEmail,password,repeatPassword,tipoUsuario)">
          <ion-list>
            <ion-item>
              <ion-label position="floating" for="nombre de usuario">Nombre de usuario</ion-label>
              <ion-input type="text" id="username" required="true" v-model="username" />
            </ion-item>
            <ion-item>
              <ion-label position="floating" for="email">Email</ion-label>
              <!-- <ion-input type="text" name="email" id="email" v-model="email" /> -->
              <ion-input type="text" id="email" v-model="email" />
              <!-- <span v-if="!v$.email.required">Email is required</span>
              <span v-if="!v$.email.email">Email is invalid</span> -->
            </ion-item>
            <ion-item>
              <ion-label position="floating" for="email">Confirmar email</ion-label>
              <ion-input type="text" id="email" required="true" v-model="repeatEmail" />
            </ion-item>
            <ion-item>
              <ion-label position="floating" for="password" >Contraseña</ion-label>
              <ion-input type="password" id="password" required="true" v-model="password" />
            </ion-item>
            <ion-item>
              <ion-label position="floating" for="password">Confirmar contraseña</ion-label>
              <ion-input type="password" id="password" required="true" v-model="repeatPassword" />
            </ion-item>
              <ion-item>
                <ion-label position="floating" for="Seleccionar tipo de usuario">Seleccionar tipo de usuario</ion-label>
                 <ion-select id="customPopoverSelect" interface="popover" placeholder="-" v-model="tipoUsuario">
                  <ion-select-option value="estudiante">Estudiante</ion-select-option>
                  <ion-select-option value="docente">Docente</ion-select-option>
                </ion-select>
              </ion-item>
            </ion-list>
            <ion-button type="submit">Terminar registro</ion-button>
        </form>
    </tabs-base>
</template>

<script>
import TabsBase from '../components/base/TabsBase.vue';
import { IonInput, IonLabel, IonButton, IonList, IonItem, IonSelect, IonSelectOption, } from '@ionic/vue';
import axios from 'axios';
import { loading } from './overlay-views/loading.js';
//import useVuelidate from '@vuelidate/core'
//import { required, email } from '@vuelidate/validators'

export default {
  /*data(){
    return{
      email: '',
    }
  },*/
    components: {
        TabsBase,
        IonInput,
        IonLabel,
        IonButton,
        IonList, 
        IonItem, 
        IonSelect, 
        IonSelectOption,
    },
    methods: {
    onSubmit(username,email,repeatEmail,password,repeatPassword,tipoUsuario) {
      let esDocente = false;
      console.log(username);
      console.log(email);
      console.log(repeatEmail);
      console.log(password);
      console.log(repeatPassword);
      console.log(tipoUsuario);
      if(email != repeatEmail ){
        console.log("Los email no coinciden!");
        return false;
      }
      if(password != repeatPassword ){
        console.log("Las contraseñas no coinciden!");
        return false;
      }
      if(tipoUsuario == "docente"){
        esDocente = true;
      }
      this.postUser(username, email, password, esDocente);
    },

    postUser(username, email, password, esDocente){
      loading.showMsg("Creando usuario...");
        axios.post("/api/register", {
                "username": username,
                "email": email,
                "password": password,
                "esDocente": esDocente ,
        }).then(resp => {
            loading.hide();
            console.log(resp);
        }).catch(err => {
            loading.hide();
            console.log(err);
        })
    }
  },
  mounted() {
    const customPopoverSelect = document.getElementById('customPopoverSelect');
    const customPopoverOptions = {
      header: 'Tipo de usario',
      //subHeader: 'Select your hair color',
      message: 'Seleccione el tipo de usuario de la cuenta que desea registrar.'
    };
    customPopoverSelect.interfaceOptions = customPopoverOptions;

    const selectEl = document.querySelector('ion-select');

    const valueLabel = document.querySelector('ion-select-option');
    selectEl.addEventListener('ionChange', () => {
    valueLabel.innerHTML = JSON.stringify(selectEl.value);
    console.log(valueLabel);
  });
  },
  /*validations () {
    return {
      /*firstName: { required }, // Matches this.firstName
      lastName: { required }, // Matches this.lastName
      contact: {
        email: { required, email } // Matches this.contact.email
      //}
    }
  },
  setup () {
    return { v$: useVuelidate() }
  },*/
}

</script>

<style scoped>

ion-button{
  width: 100%;
  margin-top: 10%;
}

/*ion-input{
  padding: 12px 20px;
  margin: 8px 0;
  box-sizing: border-box;
  border: 3px solid #ccc;
  -webkit-transition: 0.5s;
  transition: 0.5s;
  outline: none;
}
ion-input:focus {
  border: 3px solid #555;
}*/
</style>