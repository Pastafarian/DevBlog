import { Component, OnInit } from '@angular/core';
import { SetBreadCrumb } from '@app/store/breadcrumb/breadcrumb.actions';
import { Store } from '@ngxs/store';

@Component({
  selector: 'app-hire-me',
  templateUrl: './hire-me.component.html',
  styleUrls: ['./hire-me.component.scss']
})
export class HireMeComponent implements OnInit {

  constructor(private store: Store) { }

  ngOnInit(): void {

    this.store.dispatch(
      new SetBreadCrumb({
        breadCrumbs: [
          { name: 'Home', link: '/' },
          { name: 'Hire Me', last: true },
        ]
      })
    );
  }

}
