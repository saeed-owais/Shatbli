import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';
import { AuthResponse, User, LoginRequest, RegisterRequest, RegisterResponse } from '../../shared/models/user.model';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly apiUrl = environment.apiUrl;

  currentUserSig = signal<User | undefined | null>(undefined);
  isLoggedIn = computed(() => !!this.currentUserSig());

  constructor(private http: HttpClient) {
    this.loadUserFromStorage();
  }

  private loadUserFromStorage(): void {
    try {
      const userJson = localStorage.getItem('user');
      const token = localStorage.getItem('token');

      // التحقق من أن القيم موجودة وليست undefined
      if (userJson && userJson !== 'undefined' && userJson !== 'null' &&
        token && token !== 'undefined' && token !== 'null') {
        const user = JSON.parse(userJson);
        user.token = token;
        this.currentUserSig.set(user);
      } else {
        // مسح البيانات غير الصالحة
        this.logout();
      }
    } catch {
      // في حالة حدوث خطأ في JSON.parse
      this.logout();
    }
  }

  login(credentials: LoginRequest) {
    return this.http.post<AuthResponse>(`${this.apiUrl}/Users/login`, credentials)
      .pipe(
        tap(res => {
          this.saveAuthData(res);
        })
      );
  }

  register(data: RegisterRequest) {
    return this.http.post<RegisterResponse>(`${this.apiUrl}/Users/register`, data);
  }

  private saveAuthData(res: AuthResponse): void {
    // التأكد من نجاح العملية ووجود البيانات قبل حفظها
    if (res?.success && res.data?.token) {
      const user: User = {
        id: '', // الـ API الجديد لا يرجع userId
        email: res.data.email,
        fullName: res.data.fullName,
        role: res.data.role,
        token: res.data.token
      };
      localStorage.setItem('user', JSON.stringify(user));
      localStorage.setItem('token', res.data.token);
      this.currentUserSig.set(user);
    }
  }

  logout(): void {
    localStorage.removeItem('user');
    localStorage.removeItem('token');
    this.currentUserSig.set(null);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }
}