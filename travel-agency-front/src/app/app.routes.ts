import { Routes } from '@angular/router';
import { UserPageComponent } from './components/user-page/user-page.component';
import { ListHotelsComponent } from './components/lists/list-hotels/list-hotels.component';
import { ListTransportsComponent } from './components/lists/list-transports/list-transports.component';
import { CreateHotelComponent } from './components/creation/create-hotel/create-hotel.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { MainPageComponent } from './components/main-page/main-page.component';
import { ListPlacesComponent } from './components/lists/list-places/list-places.component';
import { ListUnavailableToursComponent } from './components/lists/list-unavailable-tours/list-unavailable-tours.component';
import { UpdateHotelComponent } from './components/update/update-hotel/update-hotel.component';
import { TourComponent } from './components/tour/tour.component';
import { CreatePlaceComponent } from './components/creation/create-place/create-place.component';
import { CreateTourComponent } from './components/creation/create-tour/create-tour.component';
import { TestComponent } from './components/test/test.component';

export const routes: Routes = [
    {path: '', component: MainPageComponent},
    {path: 'user', component: UserPageComponent},
    {path: 'hotels', component: ListHotelsComponent},
    {path: 'transports', component: ListTransportsComponent},
    {path: 'places', component: ListPlacesComponent},
    {path: 'unavailable-tours', component: ListUnavailableToursComponent},
    {path: 'tour/:id', component: TourComponent},
    {path: 'update-hotel/:id', component: UpdateHotelComponent},
    {path: 'create-hotel', component: CreateHotelComponent},
    {path: 'create-place', component: CreatePlaceComponent},
    {path: 'create-tour', component: CreateTourComponent},
    {path: 'test', component: TestComponent},
    {path: '**', component: PageNotFoundComponent}
];
