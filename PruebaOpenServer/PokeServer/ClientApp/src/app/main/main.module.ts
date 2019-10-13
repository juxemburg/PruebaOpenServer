import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MainRoutingModule } from './main-routing.module';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { SharedModule } from '../shared/shared.module';
import { NavbarComponent } from './components/navbar/navbar.component';

@NgModule({
  declarations: [DashboardComponent, SidebarComponent, NavbarComponent],
  imports: [CommonModule, SharedModule, MainRoutingModule],
})
export class MainModule {}
