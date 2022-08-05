<template>
    <tabs-base page-title="Iniciar sesión">
        <form class="ion-padding" autocomplete="on" @submit.prevent="login()">
            <ion-item>
                <ion-label position="floating" for="username">Nombre de usuario</ion-label>
                <ion-input type="text" v-model="usuario.username" />
            </ion-item>
            <ion-item>
                <ion-label position="floating" for="contraseña">Contraseña</ion-label>
                <ion-input type="password" v-model="usuario.password" />
            </ion-item>
            <ion-button type="submit">Ingresar</ion-button>
            <br>
            <a href="/signup">¿Todavía no tenés una cuenta?</a>
        </form>
    </tabs-base>
</template>

<script>
import TabsBase from '../components/base/TabsBase.vue';
import { IonInput, IonLabel, IonButton, IonItem, } from '@ionic/vue';
import axios from 'axios';
import { useRouter } from 'vue-router';
import { loading } from './overlay-views/loading.js';
import { alertDialog } from './overlay-views/alertDialog.js';

export default {
    components: {
        TabsBase,
        IonInput,
        IonLabel,
        IonButton,
        IonItem,
    },
    data(){
        return{
            usuario: {
                "username": '',
                "password": '',
            }
        }
    },
    methods:{
        // https://www.techiediaries.com/vue-vuex-axios-auth/
        login(){
            loading.showMsg("Ingresando...");
            axios.post("/api/login", this.usuario).then( resp => {
                loading.hide();
                if(resp.data.isValid){
                    this.$store.dispatch("usuario/login", resp.data);
                    localStorage.setItem('token', resp.data.authToken);
                    localStorage.setItem('usuario', JSON.stringify(resp.data.user));
                    this.redirect();
                    this.cleanForm();
                }else{
                    switch (resp.data.status) {
                        case 1:
                            alertDialog.showAlertMsg('Nombre de usuario inválido.')
                            break;
                        case 2:
                            alertDialog.showAlertMsg('Contraseña inválida.')
                            break;
                    
                        default:
                            break;
                    }
                }
            });
        },

        cleanForm(){
            this.usuario.username = '';
            this.usuario.password = '';
        }
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

.button{
    width: 100%;
}

</style>