import { createRouter, createWebHistory } from 'vue-router';
import MainPage from '@/../src/components/MainPage.vue';
import CreateHotel from '@/components/CreatePages/CreateHotel.vue';
import CreateTransport from '@/components/CreatePages/CreateTransport.vue';
import CreateTour from '@/components/CreatePages/CreateTour.vue';
import CreatePlace from '@/components/CreatePages/CreatePlace.vue';
import TourPage from '@/../src/components/TourPage.vue';
import UpdateTourPage from '@/components/UpdatePages/UpdateTour.vue';
import UpdateHotelPage from '@/components/UpdatePages/UpdateHotel.vue';
import UpdateTransportPage from '@/components/UpdatePages/UpdateTransport.vue';
import UpdatePlacePage from '@/components/UpdatePages/UpdatePlace.vue';


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
    },
    {
        path: '/tour/:id',
        name: 'TourPage',
        component: TourPage,
    },
    {
        path: '/updateTour/:id',
        name: 'UpdateTourPage',
        component: UpdateTourPage,
    },
    {
        path: '/updateHotel/:id',
        name: 'UpdateHotelPage',
        component: UpdateHotelPage,
    },
    {
        path: '/updateTransport/:id',
        name: 'UpdateTransportPage',
        component: UpdateTransportPage,
    },
    {
        path: '/updatePlace/:id',
        name: 'UpdatePlacePage',
        component: UpdatePlacePage,
    },
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