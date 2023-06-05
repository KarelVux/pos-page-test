import {createRouter, createWebHistory} from 'vue-router'
import HomeView from '../views/HomeView.vue'
import BusinessView from "@/views/store/BusinessView.vue";
import BusinessDetailsView from "@/views/store/BusinessDetailsView.vue";
import InvoiceOrderView from "@/views/store/InvoiceOrderView.vue";
import InvoiceHistoriesView from "@/views/store/InvoiceHistoriesView.vue";
import UserBusinessesView from "@/views/manager/UserBusinessesView.vue";
import UserBusinessManagementView from "@/views/manager/UserBusinessManagementView.vue";
import InvoiceAcceptanceView from "@/views/store/InvoiceAcceptanceView.vue";
import InvoiceDetailsView from "../views/store/InvoiceDetailsView.vue";

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        {
            path: '/',
            name: 'home',
            component: HomeView
        },
        {
            path: '/about',
            name: 'about',
            // route level code-splitting
            // this generates a separate chunk (About.[hash].js) for this route
            // which is lazy-loaded when the route is visited.
            component: () => import('../views/AboutView.vue')
        },
        {
            path: '/store/business',
            name: 'business',
            component: BusinessView
        },
        {
            path: '/store/businessDetails/:id',
            name: 'businessDetails',
            component: BusinessDetailsView,
            props: true
        },
        {
            path: '/store/invoiceDetails/:id',
            name: 'invoiceDetails',
            component: InvoiceDetailsView,
            props: true
        },
        {
            path: '/store/invoiceAcceptance/:id',
            name: 'invoiceAcceptance',
            component: InvoiceAcceptanceView,
            props: true
        }
        ,
        {
            path: '/store/invoiceOrder/:id',
            name: 'invoiceOrder',
            component: InvoiceOrderView,
            props: true
        },
        {
            path: '/store/invoiceHistories',
            name: 'invoiceHistories',
            component: InvoiceHistoriesView,
            props: true
        },
        {
            path: '/manager/userBusinesses',
            name: 'userBusinesses',
            component: UserBusinessesView,
        },
        {
            path: '/manager/userBusinesses/:id',
            name: 'userBusinessesManagement',
            component: UserBusinessManagementView,
        }
    ]
})

export default router
