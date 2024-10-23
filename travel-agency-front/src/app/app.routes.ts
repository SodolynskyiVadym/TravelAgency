import { Routes } from '@angular/router';
import { MainPageComponent } from './main-page/main-page.component';
import { UserPageComponent } from './user-page/user-page.component';

export const routes: Routes = [
    {path: '', component: MainPageComponent},
    {path: 'user', component: UserPageComponent}
];
