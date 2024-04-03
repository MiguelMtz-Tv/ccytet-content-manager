import { createApp } from 'vue'
import { createRouter, createWebHistory } from 'vue-router'
import router from './routing'
import './styles.css'
import Antd from 'ant-design-vue'
import App from './App.vue'
import CKEditor from '@ckeditor/ckeditor5-vue'
import 'vuetify/styles'
import { createVuetify } from 'vuetify'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'

const app = createApp(App)

const vuetify = createVuetify({
    components,
    directives,
})

app.use(CKEditor)
app.use(router)
app.use(Antd)

app.use(vuetify)

app.mount('#app')

