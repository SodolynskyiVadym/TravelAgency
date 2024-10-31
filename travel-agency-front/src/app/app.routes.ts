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
import { LoginComponent } from './components/login/login.component';
import { AuthGuardService } from '../services/auth-guard.service';
import { SignupComponent } from './components/signup/signup.component';
import { AdminComponent } from './components/admin/admin.component';

export const routes: Routes = [
    {path: '', component: MainPageComponent},
    {path: 'user', component: UserPageComponent, canActivate: [AuthGuardService], data: {expectedRoles: ['ADMIN', 'EDITOR', 'USER']}},
    {path: 'hotels', component: ListHotelsComponent, canActivate: [AuthGuardService], data: {expectedRoles: ['ADMIN', 'EDITOR']}},
    {path: 'transports', component: ListTransportsComponent, canActivate: [AuthGuardService], data: {expectedRoles: ['ADMIN', 'EDITOR']}},
    {path: 'places', component: ListPlacesComponent, canActivate: [AuthGuardService], data: {expectedRoles: ['ADMIN', 'EDITOR']}},
    {path: 'unavailable-tours', component: ListUnavailableToursComponent, canActivate: [AuthGuardService], data: {expectedRoles: ['ADMIN', 'EDITOR']}},
    {path: 'tour/:id', component: TourComponent},
    {path: 'update-hotel/:id', component: UpdateHotelComponent, canActivate: [AuthGuardService], data: {expectedRoles: ['ADMIN', 'EDITOR']}},
    {path: 'create-hotel', component: CreateHotelComponent, canActivate: [AuthGuardService], data: {expectedRoles: ['ADMIN', 'EDITOR']}},
    {path: 'create-place', component: CreatePlaceComponent, canActivate: [AuthGuardService], data: {expectedRoles: ['ADMIN', 'EDITOR']}},
    {path: 'create-tour', component: CreateTourComponent, canActivate: [AuthGuardService], data: {expectedRoles: ['ADMIN', 'EDITOR']}},
    {path: 'login', component: LoginComponent},
    {path: 'signup', component: SignupComponent},
    {path: 'admin', component: AdminComponent, canActivate: [AuthGuardService], data: {expectedRoles: ['ADMIN']}},
    {path: 'test', component: TestComponent},
    {path: '**', component: PageNotFoundComponent}
];
