import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GameRoutingModule } from './game-routing.module';
import { DashboardComponent } from './dashboard/dashboard.component';


@NgModule({
    declarations: [
        DashboardComponent
    ],
    imports: [
        CommonModule,
        GameRoutingModule
    ]
})
export class GameModule { }
