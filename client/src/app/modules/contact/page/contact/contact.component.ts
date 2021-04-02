import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { ApiService } from '@data/services/api.service';
import { trigger, transition, state, style, animate } from '@angular/animations';
import { Store } from '@ngxs/store';
import { SetBreadCrumb } from '@store/breadcrumb/breadcrumb.actions';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

interface Vm {
  form: FormGroup;
}

@UntilDestroy()
@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.scss'],
  animations: [
    trigger('swooshInOutAnimation', [
      state(
        'active',
        style({
          opacity: 1,
          display: 'block',
          position: 'relative',
          transform: 'translateX(0%)'
        })
      ),
      state(
        'inactive',
        style({
          opacity: 0.2,
          display: 'none',
          position: 'absolute',
          top: 0,
          left: 0,
          transform: 'translateX(-100%)'
        })
      ),
      transition('active => inactive', animate('390ms ease-in')),
      transition('inactive => active', animate('390ms ease-in'))
    ])
  ]
})
export class ContactComponent implements OnInit {

  form: FormGroup;
  vm$: Observable<Vm>;

  beforeSubmitState = 'inactive';
  afterSubmitState = 'active';

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private store: Store) { }

  ngOnInit() {

    this.vm$ = this.store.dispatch(
      new SetBreadCrumb({
        breadCrumbs: [
          { name: 'Home', link: '/' },
          { name: 'Contact', last: true }
        ]
      })
    ).pipe(
      map(() => {
        return {
          form: this.fb.group({
            name: ['', [Validators.required, Validators.maxLength(25), Validators.minLength(3)]],
            email: ['', [Validators.required, Validators.maxLength(130), Validators.email]],
            message: ['', [Validators.required, Validators.maxLength(200), Validators.minLength(10)]]
          })
        };
      }),
      untilDestroyed(this)
    );
  }

  showHide() {
    this.beforeSubmitState = this.beforeSubmitState === 'active' ? 'inactive' : 'active';
    this.afterSubmitState = this.afterSubmitState === 'inactive' ? 'active' : 'inactive';
  }

  save(form: FormGroup) {
    if (form.valid) {

      const contact = form.value;
      form.reset(contact);

      this.apiService.postContact(contact).subscribe(_ => {
        this.beforeSubmitState = this.beforeSubmitState === 'active' ? 'inactive' : 'active';
        this.afterSubmitState = this.afterSubmitState === 'active' ? 'inactive' : 'active';
      });
    }
  }
}
