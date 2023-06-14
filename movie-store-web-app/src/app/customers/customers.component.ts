import { Component, OnInit } from '@angular/core';
import { CustomerClient, Customer, DeleteCustomerCommand } from '../api/api-reference';
import { Router } from '@angular/router';

@Component({
  selector: 'app-customers',
  templateUrl: './customers.component.html',
  styleUrls: ['./customers.component.css']
})
export class CustomersComponent implements OnInit{

  displayedColumns: string[] = ['id', 'name', 'email', 'role', 'status', 'statusExpirationDate', 'actions'];

  customers: Customer[] = [];
  
  constructor(private client: CustomerClient, private router: Router) {}

  ngOnInit(){
    this.client.getAll().subscribe(result => {
      this.customers = result;
    });
  }

  onDelete(query: DeleteCustomerCommand){
    this.client.delete(query).subscribe(_ => {
      this.customers = this.customers.filter(x => x.id !== query.id)
    })
  }

  onUpdate(customer: Customer) {
    this.router.navigate([`/customers/edit/${customer.id}`]);
  }


}
