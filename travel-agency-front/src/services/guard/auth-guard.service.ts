import { Injectable } from '@angular/core';
import { AuthService } from '../auth/auth.service';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthGuardService implements CanActivate {

  constructor(private authService: AuthService, private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const expectedRoles = route.data['expectedRoles'];
    const userRole = this.authService.getUserRole();

    if (!this.authService.isAuthenticated() || expectedRoles.includes(userRole)) {
      this.router.navigate(['/login']);
      return false;
    }
    return true;
  }
}
