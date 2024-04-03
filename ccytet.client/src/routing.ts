import HelloWorld   from './components/HelloWorld.vue'
import IndexNoticia from './pages/Noticias/IndexNoticia.vue'
import CrearNoticia from './pages/Noticias/CrearNoticia.vue'
import IndexConvocatoria from './pages/Convocatorias/IndexConvocatoria.vue'
import CrearConvocatoria from './pages/Convocatorias/CrearConvocatoria.vue'
import IndexESF from './pages/EstadosSituacionFinanciera/IndexESF.vue'
import CrearESF from './pages/EstadosSituacionFinanciera/CrearESF.vue'
import SignIn from './pages/auth/SignIn.vue'
import SignUp from './pages/auth/SignUp.vue'
import { createRouter, createWebHistory } from 'vue-router'
import { Sessions } from './libraries/sessions'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    //Auth
    { path: '/sign-in',               component:  SignIn, meta: { auth: false }},
    { path: '/sign-up',               component:  SignUp, meta: { auth: false }},
    
    //Inicio
    { path: '/',                      component:  HelloWorld, meta: { auth: true }},

    //noticias
    { path: '/noticias',              component:  IndexNoticia, meta: { auth: true } },
    { path: '/noticias/crear',        component:  CrearNoticia, meta: { auth: true } },

    //Convocatorias
    { path: '/convocatorias',         component:  IndexConvocatoria, meta: { auth: true } },
    { path: '/convocatorias/crear',   component:  CrearConvocatoria, meta: { auth: true } },

    //Estados de situación financiera
    { path: '/esf',                   component:  IndexESF, meta: { auth: true } },
    { path: '/esf/crear',             component:  CrearESF, meta: { auth: true } },
  ]
})

router.beforeEach((to, from, next) => {
  if(!Sessions.check() && to.meta.auth){
    next('/sign-in')
  }else if(Sessions.check() && !to.meta.auth){
    next('/')
  }else{
    next()
  }
})

export default router;