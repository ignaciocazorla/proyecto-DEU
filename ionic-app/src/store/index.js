import { createStore } from 'vuex';

const cursos = {
    namespaced: true,
    state () {
        return {
            cursos: []
        };
    },
    getters: {
        cursos(state) {
            return state.cursos;
        },
        curso(state) {
            return (cursoId) => {
                return state.cursos.find((curso) => curso.id == cursoId);
            };
        },
    },
    mutations: {
        load(state, data){
            state.cursos = data;
        },
        add(state, curso){
            state.cursos.push(curso);
        },
        delete(state, id){
            state.cursos = state.cursos.filter((curso) => curso.id != id);
        }
    },
    actions:{
        load(context, id){
            context.commit('load', id);
        },
        add(context, id){
            context.commit('add', id);
        },
        delete(context, id){
            context.commit('delete', id);
        }
    }
};

const recursos = {
    namespaced: true,
    state () {
        return {
            recursos: []
        };
    },
    getters: {
        recursos(state) {
            return state.recursos;
        },
        recurso(state) {
            return (recursoId) => {
                return state.recursos.find((recurso) => recurso.id === recursoId);
            };
        },
    },
    mutations: {
        load(state, data){
            state.recursos = data;
        },
        add(state, recurso){
            state.recursos.push(recurso);
        },
        delete(state, id){
            state.recursos = state.recursos.filter((recurso) => recurso.id != id);
        }
    },
    actions:{
        load(context, id){
            context.commit('load', id);
        },
        add(context, id){
            context.commit('add', id);
        },
        delete(context, id){
            context.commit('delete', id);
        }
    }
}

const usuario = {
    namespaced: true,
    state () {
        return {
            usuario: null,
            authToken: null,
        };
    },
    getters: {
        usuario(state) {
            return state.usuario;
        },
        token(state){
            return state.authToken;
        }
    },
    mutations: {
        login(state, data){
            state.usuario = data.username;
            state.authToken = data.authToken;
        },
        add(state, recurso){
            state.recursos.push(recurso);
        },
        delete(state, id){
            state.recursos = state.recursos.filter((recurso) => recurso.id != id);
        }
    },
    actions:{
        login(context, data){
            context.commit('login', data);
        },
        add(context, id){
            context.commit('add', id);
        },
        delete(context, id){
            context.commit('delete', id);
        }
    }
}

const store = createStore({
    modules:{
        cursos: cursos,
        recursos: recursos,
        usuario: usuario,
    }
});

export default store;