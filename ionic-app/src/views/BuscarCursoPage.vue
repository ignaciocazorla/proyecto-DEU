<template>
    <ion-page>
        <ion-header>
            <ion-toolbar>Buscar cursos</ion-toolbar>
        </ion-header>
        <ion-content>
            <ion-searchbar animated></ion-searchbar>
        </ion-content>
    </ion-page>
</template>

<script>
import {IonPage, IonHeader, IonToolbar, IonContent, IonSearchbar} from '@ionic/vue';
export default {
    components: {IonPage, IonHeader, IonToolbar, IonContent, IonSearchbar},
    
}
</script>