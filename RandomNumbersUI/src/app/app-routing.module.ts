import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: 'auth',
    loadChildren: () => import('./components/auth/auth.module').then(m => m.AuthModule)
  },
  {
    path: 'game',
    loadChildren: () => import('./components/game/game.module').then(m => m.GameModule)
  },
  {
    path: '',
    redirectTo: '/game/dashboard',
    pathMatch: 'full'
  },
  {
    path: '**',
    redirectTo: '/game/dashboard'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
