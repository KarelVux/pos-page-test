import {createApp} from 'vue';
import {createPinia} from 'pinia';

import App from './App.vue';
import router from './router';
// import 'vuetify/styles';
// import {createVuetify} from 'vuetify';
// import * as components from 'vuetify/components';
// import * as directives from 'vuetify/directives';

import "bootstrap/dist/css/bootstrap.min.css"
import "bootstrap"
import 'jquery';
import 'font-awesome/css/font-awesome.min.css';
import 'bootstrap-icons/font/bootstrap-icons.css'

const app = createApp(App);

// const vuetify = createVuetify({
//     components,
//     directives
// });

app.use(createPinia());
app.use(router);
// app.use(vuetify);
app.mount('#app');
