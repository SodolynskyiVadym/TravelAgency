import { createRouter, createWebHistory } from 'vue-router';
import MainPage from '@/../src/components/MainPage.vue';
import CreateHotel from '@/../src/components/CreateHotel.vue';
import CreateTransport from '@/../src/components/CreateTransport.vue';
import CreateTour from '@/../src/components/CreateTour.vue';
import CreatePlace from '@/../src/components/CreatePlace.vue';


// import * as listURL from "@/js/listUrl";


const routes = [
    {
        path: '/',
        name: 'MainPage',
        component: MainPage,
    },
    {
        path: '/createHotel',
        name: 'CreateHotelPage',
        component: CreateHotel,
    },
    {
        path: '/createTransport',
        name: 'CreateTransportPage',
        component: CreateTransport,
    },
    {
        path: '/createTour',
        name: 'CreateTourPage',
        component: CreateTour,
    },
    {
        path: '/createPlace',
        name: 'CreatePlacePage',
        component: CreatePlace,
    }
];


const router = createRouter({
    history: createWebHistory(),
    routes
});



// router.beforeEach(async (to, from, next) => {
//     if (to.meta.requiresAuth) {
//         const token = localStorage.getItem('token');
//         if (token) {
//             const userData = await listURL.getUserByToken(token);
//             const role = userData.role;

//             if (to.meta.roles.includes(role)) {
//                 next();
//             } else {
//                 next("/login");
//             }
//         } else {
//             next('/login');
//         }
//     }else {
//         next();
//     }
// });

export default router;