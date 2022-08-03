import { createRouter, createWebHistory } from '@ionic/vue-router';
import TabsPage from '../views/TabsPage.vue'
import WelcomePage from '../views/WelcomePage.vue'
import store from '../store/index.js';

function isAuthenticated(){
  let token = localStorage.getItem("token");
  return (token != undefined);
}

function setStore(){
  let token = localStorage.getItem("token");
  let usuario = localStorage.getItem("usuario");
  if(store.getters["usuario/token"] == undefined){
    store.dispatch("usuario/login", {
      "authToken": token,
      "user": JSON.parse(usuario),
  });
  }
}

const routes = [
  {
    path: '/',
    component: WelcomePage,
    
  },
  {
    path: '/login',
    component: () => import('../views/LoginPage.vue')
  },
  {
    path: '/signup',
    component: () => import('../views/SignupPage.vue')
  },
  {
    path: '/tabs/',
    component: TabsPage,
    children: [
      {
        path: '',
        redirect: '/tabs/tab1'
      },
      {
        path: 'tab1',
        component: () => import('@/views/Tab1Page.vue')
      },
      {
        path: 'cursos',
        component: () => import('@/views/Tab2Page.vue'),
        meta: {requiresLogin:true},
      },
      {
        path: 'cursos/search',
        component: () => import('../views/cursos/BuscarCursoPage.vue'),
        meta: {requiresLogin:true},
      },
      {
        path: 'cursos/:id',
        component: () => import('@/views/cursos/DetalleCursos.vue'),
        meta: {requiresLogin:true},
      },
      {
        path: 'cursos/recursos/add',
        component: () => import('@/views/recursos/CrearRecursoPage.vue'),
        meta: {requiresLogin:true},
      },
      {
        path: 'cursos/recursos/:id',
        component: () => import('@/views/recursos/DetalleRecursoPage.vue'),
        meta: {requiresLogin:true},
      },
      {
        path: 'cursos/add',
        component: () => import('@/views/cursos/CrearCursoPage.vue'),
        meta: {requiresLogin:true},
      },
      {
        path: 'cursos/update/:id',
        component: () => import('@/views/cursos/ModificarCursoPage.vue'),
        meta: {requiresLogin:true},
      },
      {
        path: 'ajustes',
        component: () => import('@/views/Tab3Page.vue'),
        meta: {requiresLogin:true},
      }
    ]
  }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

router.beforeEach((to, from, next) => {
  if (to.matched.some(record => record.meta.requiresLogin) && !(isAuthenticated())) {
      next("/login")
  } else {
      setStore();
      next();
  }
})

export default router
