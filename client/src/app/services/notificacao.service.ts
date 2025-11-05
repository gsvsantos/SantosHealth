import { MatSnackBar } from '@angular/material/snack-bar';
import { inject, Injectable } from '@angular/core';

@Injectable()
export class NotificacaoService {
  protected readonly snackBar = inject(MatSnackBar);

  public sucesso(mensagem: string, acao: string): void {
    this.snackBar.open(mensagem, acao, {
      panelClass: ['notificacao-sucesso'],
    });
  }
  public alerta(mensagem: string, acao: string): void {
    this.snackBar.open(mensagem, acao, {
      panelClass: ['notificacao-alerta'],
    });
  }
  public erro(mensagem: string, acao: string): void {
    this.snackBar.open(mensagem, acao, {
      panelClass: ['notificacao-erro'],
    });
  }
}
