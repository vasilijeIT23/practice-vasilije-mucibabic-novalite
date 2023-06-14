import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { Customer, CustomerClient, Role, Status, UpdateCustomerCommand } from 'src/app/api/api-reference';

@Component({
  selector: 'app-edit-customer',
  templateUrl: './edit-customer.component.html',
  styleUrls: ['./edit-customer.component.css']
})
export class EditCustomerComponent implements OnInit{
  role = new Map<Role, string>([
    [Role.Admin, 'Admin'],
    [Role.Regular, 'Regular']
  ]);

  status = new Map<Status, string>([
    [Status.Advanced, 'Advanced'],
    [Status.Regular, 'Regular']
  ])  

  constructor(private route: ActivatedRoute,
    private client: CustomerClient,
    private router: Router,
    private readonly snackBar: MatSnackBar) { }

  id: string = '';

  formGroup = new FormGroup({
    id: new FormControl('', { nonNullable: true }),
    name: new FormControl('', { nonNullable: true }),
    email: new FormControl('',{ nonNullable: true , validators: [Validators.required, Validators.email]}),
    role: new FormControl<Role>(Role.Regular, { nonNullable: true }),
  });

  ngOnInit(){
    this.id = this.route.snapshot.paramMap.get('id')!;
    this.client.getById(this.id).subscribe(data => {
      this.patchForm(data);
    });
  }
  onSubmit(){
    this.client.update(new UpdateCustomerCommand({
        id: this.formGroup.controls.id.value,
        name: this.formGroup.controls.name.value,
        email: this.formGroup.controls.email.value,
        role: this.formGroup.controls.role.value
    })).subscribe(_ => {
        this.snackBar.open('Customer updated');
        this.router.navigate([`/customers/`]);
    });
  }

  get email(){
    return this.formGroup.controls.email;
  }
  
  private readonly patchForm = (customer: Customer) => {
    this.formGroup.controls.id.patchValue(this.id);
    this.formGroup.controls.name.patchValue(customer.name!);
    this.formGroup.controls.email.patchValue(customer.email!);
    this.formGroup.controls.role.patchValue(customer.role!);
  }
}
