import { LoginnedTokens } from './loginned-tokens';
import { DataBaseSource } from 'src/app/Models/NotificationSources/database-source';

export class Loginned {
    tokens: LoginnedTokens
    role: string;
    panels: string[];
    permissions: string[];
    notifications: DataBaseSource[];
}