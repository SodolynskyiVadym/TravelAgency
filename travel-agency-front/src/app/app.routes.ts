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
import { ForgotPasswordComponent } from './components/forgot-password/forgot-password.component';
import { UpdatePlaceComponent } from './components/update/update-place/update-place.component';
import { UpdateTourComponent } from './components/update/update-tour/update-tour.component';
import { UpdateTransportComponent } from './components/update/update-transport/update-transport.component';
import { CreateTransportComponent } from './components/creation/create-transport/create-transport.component';

export const routes: Routes = [
    {path: '', component: MainPageComponent},
    {path: 'user', component: UserPageComponent, canActivate: [AuthGuardService], data: {expectedRoles: ['ADMIN', 'EDITOR', 'USER']}},
    {path: 'hotels', component: ListHotelsComponent, canActivate: [AuthGuardService], data: {expectedRoles: ['ADMIN', 'EDITOR']}, pathMatch: 'full'},
    {path: 'transports', component: ListTransportsComponent, canActivate: [AuthGuardService], data: {expectedRoles: ['ADMIN', 'EDITOR']}, pathMatch: 'full'},
    {path: 'places', component: ListPlacesComponent, canActivate: [AuthGuardService], data: {expectedRoles: ['ADMIN', 'EDITOR']}, pathMatch: 'full'},
    {path: 'unavailable-tours', component: ListUnavailableToursComponent, canActivate: [AuthGuardService], data: {expectedRoles: ['ADMIN', 'EDITOR']}, pathMatch: 'full'},
    {path: 'tour/:id', component: TourComponent},
    {path: 'update-hotel/:id', component: UpdateHotelComponent, canActivate: [AuthGuardService], data: {expectedRoles: ['ADMIN', 'EDITOR']}},
    {path: 'create-hotel', component: CreateHotelComponent, canActivate: [AuthGuardService], data: {expectedRoles: ['ADMIN', 'EDITOR']}},
    {path: 'create-place', component: CreatePlaceComponent, canActivate: [AuthGuardService], data: {expectedRoles: ['ADMIN', 'EDITOR']}},
    {path: 'create-transport', component: CreateTransportComponent, canActivate: [AuthGuardService], data: {expectedRoles: ['ADMIN', 'EDITOR']}},
    {path: 'create-tour', component: CreateTourComponent, canActivate: [AuthGuardService], data: {expectedRoles: ['ADMIN', 'EDITOR']}},
    {path: 'update-place/:id', component: UpdatePlaceComponent, canActivate: [AuthGuardService], data: {expectedRoles: ['ADMIN', 'EDITOR']}},
    {path: 'update-tour/:id', component: UpdateTourComponent, canActivate: [AuthGuardService], data: {expectedRoles: ['ADMIN', 'EDITOR']}},
    {path: 'update-transport/:id', component: UpdateTransportComponent, canActivate: [AuthGuardService], data: {expectedRoles: ['ADMIN', 'EDITOR']}},
    {path: 'update-hotel/:id', component: UpdateHotelComponent, canActivate: [AuthGuardService], data: {expectedRoles: ['ADMIN', 'EDITOR']}},
    {path: 'login', component: LoginComponent},
    {path: 'signup', component: SignupComponent},
    {path: 'admin', component: AdminComponent, canActivate: [AuthGuardService], data: {expectedRoles: ['ADMIN']}},
    {path: 'forgot-password', component: ForgotPasswordComponent},
    {path: 'test', component: TestComponent},
    {path: '**', component: PageNotFoundComponent}
];
