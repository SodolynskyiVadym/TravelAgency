import { Routes } from '@angular/router';
import { UserPageComponent } from './components/user-page/user-page.component';
import { ListHotelsComponent } from './components/lists/list-hotels/list-hotels.component';
import { ListTransportsComponent } from './components/lists/list-transports/list-transports.component';
import { ListLocationsComponent } from './components/lists/list-locations/list-locations.component';
import { CreateHotelComponent } from './components/creation/create-hotel/create-hotel.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { MainPageComponent } from './components/main-page/main-page.component';

export const routes: Routes = [
    {path: '', component: MainPageComponent},
    {path: 'user', component: UserPageComponent},
    {path: 'hotels', component: ListHotelsComponent},
    {path: 'transports', component: ListTransportsComponent},
    {path: 'locations', component: ListLocationsComponent},
    {path: 'create-hotel', component: CreateHotelComponent},
    {path: '**', component: PageNotFoundComponent}
];
