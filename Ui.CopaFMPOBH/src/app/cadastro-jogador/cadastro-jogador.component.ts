import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Time } from 'src/models/time';
import { JogadorService } from 'src/services/jogador.service';
import { LoadingService } from 'src/services/loading.service';

@Component({
  selector: 'app-cadastro-jogador',
  templateUrl: './cadastro-jogador.component.html',
  styleUrls: ['./cadastro-jogador.component.css']
})
export class CadastroJogadorComponent implements OnInit {

  jogadoresForm!: FormGroup;
  jogadorForm!: FormGroup;
  ArrayFormJogadores!: FormArray;
  times!: Time[];

  constructor(private formBuilder:FormBuilder, private jogadorService:JogadorService, private loadservice:LoadingService){
  
  }

  ngOnInit(): void {
    this.jogadorForm = this.formBuilder.group({
      nome: '',
      numero: null,
      idade:null,
      igreja:'',
      IdTime:0,
      ehGoleiro:false
    });

    this.ArrayFormJogadores = this.formBuilder.array([this.jogadorForm]);

    this.jogadoresForm = this.formBuilder.group({
      jogadores: this.ArrayFormJogadores,
    });

    this.getTime();

  }

  get jogadores(){
    return this.jogadoresForm.get('jogadores') as FormArray;
  }

  addJogador(){
    this.ArrayFormJogadores.push(
      this.formBuilder.group({
        nome: '',
        numero: null,
        idade:null,
        igreja:'',
        IdTime:"",
        ehGoleiro:false
      })
    )
  }

  removeJogador(index:number){
    this.ArrayFormJogadores.removeAt(index);
  }


  getTime(){
    this.jogadorService.getTimes().subscribe(
      {
        next:(data:Time[])=>{this.times = data; console.log(this.times)},
        error:()=>{

        }
      }
    )

  }

  OnSubimit(){
    
    if(this.jogadoresForm.valid){
      this.jogadorService.createJogadores(this.jogadoresForm?.get('jogadores')?.value).subscribe({
        next:() => {
           alert('jogados cadstrados com sucesso!');
        },
        error:()=>{
          alert('não foi possivel cadastrar os jogadores');
        }
      });
    }
  }

}
