<template>
    <tabs-base page-title="Login">
        <form class="login-form-container" autocomplete="on" >
            <ion-label for="username">Username:</ion-label>
            <ion-input type="text" id="username" v-model="username" />
            <ion-label for="contraseña">Contraseña:</ion-label>
            <ion-input type="password" id="contraseña" v-model="password" />
            <!--<ion-button class="login_button" type="submit">Ingresar</ion-button> -->
            <ion-button @click="login(username, password)">Ingresar</ion-button>
            <br>
            <a href="/signup">¿Todavía no creaste un usuario? Registrar.</a>
        </form>
    </tabs-base>
</template>

<script>
import TabsBase from '../components/base/TabsBase.vue';
import { IonInput, IonLabel, IonButton, } from '@ionic/vue';
import axios from 'axios';
import { useRouter } from 'vue-router';

export default {
    components: {
        TabsBase,
        IonInput,
        IonLabel,
        IonButton
    },
    methods:{
        // https://www.techiediaries.com/vue-vuex-axios-auth/
        login(username, password){
            axios.post("/api/login", {
                "username": username,
                "password": password,
            }).then( resp => {
                console.log(resp.data);
                if(resp.data.isValid){
                    this.$store.dispatch("usuario/login", resp.data);
                    this.redirect();
                }
            });
        },
    },
    setup() {
        const router = useRouter();
        const redirect = () => {
            router.push("/tabs/cursos");
        };
        return { redirect };
    },

}
</script>

<style scoped>
.login-form-container {
  text-align: left;
  position: absolute;
  left: 0;
  right: 0;
}
</style>