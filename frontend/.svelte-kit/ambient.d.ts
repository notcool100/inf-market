
// this file is generated — do not edit it


/// <reference types="@sveltejs/kit" />

/**
 * Environment variables [loaded by Vite](https://vitejs.dev/guide/env-and-mode.html#env-files) from `.env` files and `process.env`. Like [`$env/dynamic/private`](https://kit.svelte.dev/docs/modules#$env-dynamic-private), this module cannot be imported into client-side code. This module only includes variables that _do not_ begin with [`config.kit.env.publicPrefix`](https://kit.svelte.dev/docs/configuration#env) _and do_ start with [`config.kit.env.privatePrefix`](https://kit.svelte.dev/docs/configuration#env) (if configured).
 * 
 * _Unlike_ [`$env/dynamic/private`](https://kit.svelte.dev/docs/modules#$env-dynamic-private), the values exported from this module are statically injected into your bundle at build time, enabling optimisations like dead code elimination.
 * 
 * ```ts
 * import { API_KEY } from '$env/static/private';
 * ```
 * 
 * Note that all environment variables referenced in your code should be declared (for example in an `.env` file), even if they don't have a value until the app is deployed:
 * 
 * ```
 * MY_FEATURE_FLAG=""
 * ```
 * 
 * You can override `.env` values from the command line like so:
 * 
 * ```bash
 * MY_FEATURE_FLAG="enabled" npm run dev
 * ```
 */
declare module '$env/static/private' {
	export const SHELL: string;
	export const npm_command: string;
	export const LSCOLORS: string;
	export const USER_ZDOTDIR: string;
	export const COLORTERM: string;
	export const HYPRLAND_CMD: string;
	export const VSCODE_DEBUGPY_ADAPTER_ENDPOINTS: string;
	export const LESS: string;
	export const XDG_SESSION_PATH: string;
	export const TERM_PROGRAM_VERSION: string;
	export const FNM_ARCH: string;
	export const XDG_BACKEND: string;
	export const _P9K_TTY: string;
	export const NODE: string;
	export const npm_config_ignore_scripts: string;
	export const JAVA_HOME: string;
	export const npm_config_verify_deps_before_run: string;
	export const P9K_TTY: string;
	export const npm_config__jsr_registry: string;
	export const PYDEVD_DISABLE_FILE_VALIDATION: string;
	export const FNM_NODE_DIST_MIRROR: string;
	export const DESKTOP_SESSION: string;
	export const __ETC_PROFILE_NIX_SOURCED: string;
	export const KITTY_PID: string;
	export const HL_INITIAL_WORKSPACE_TOKEN: string;
	export const NO_AT_BRIDGE: string;
	export const XDG_SEAT: string;
	export const PWD: string;
	export const NIX_PROFILES: string;
	export const LOGNAME: string;
	export const XDG_SESSION_DESKTOP: string;
	export const QT_QPA_PLATFORMTHEME: string;
	export const XDG_SESSION_TYPE: string;
	export const BUNDLED_DEBUGPY_PATH: string;
	export const _: string;
	export const KITTY_PUBLIC_KEY: string;
	export const VSCODE_GIT_ASKPASS_NODE: string;
	export const MOTD_SHOWN: string;
	export const VSCODE_INJECTION: string;
	export const HOME: string;
	export const LANG: string;
	export const FNM_COREPACK_ENABLED: string;
	export const LS_COLORS: string;
	export const _JAVA_AWT_WM_NONREPARENTING: string;
	export const XDG_CURRENT_DESKTOP: string;
	export const npm_package_version: string;
	export const STARSHIP_SHELL: string;
	export const WAYLAND_DISPLAY: string;
	export const NIX_SSL_CERT_FILE: string;
	export const KITTY_WINDOW_ID: string;
	export const GIT_ASKPASS: string;
	export const XDG_SEAT_PATH: string;
	export const pnpm_config_verify_deps_before_run: string;
	export const INIT_CWD: string;
	export const CHROME_DESKTOP: string;
	export const STARSHIP_SESSION_KEY: string;
	export const QT_QPA_PLATFORM: string;
	export const npm_lifecycle_script: string;
	export const VSCODE_GIT_ASKPASS_EXTRA_ARGS: string;
	export const XDG_SESSION_CLASS: string;
	export const ANDROID_HOME: string;
	export const TERM: string;
	export const TERMINFO: string;
	export const npm_package_name: string;
	export const ZSH: string;
	export const VSCODE_NONCE: string;
	export const ZDOTDIR: string;
	export const USER: string;
	export const npm_config_frozen_lockfile: string;
	export const VSCODE_GIT_IPC_HANDLE: string;
	export const HYPRLAND_INSTANCE_SIGNATURE: string;
	export const DISPLAY: string;
	export const npm_lifecycle_event: string;
	export const SHLVL: string;
	export const MOZ_ENABLE_WAYLAND: string;
	export const PAGER: string;
	export const ANDROID_SDK_ROOT: string;
	export const _P9K_SSH_TTY: string;
	export const FNM_VERSION_FILE_STRATEGY: string;
	export const XDG_VTNR: string;
	export const XDG_SESSION_ID: string;
	export const npm_config_user_agent: string;
	export const PNPM_SCRIPT_SRC_DIR: string;
	export const npm_execpath: string;
	export const XDG_RUNTIME_DIR: string;
	export const FNM_RESOLVE_ENGINES: string;
	export const DEBUGINFOD_URLS: string;
	export const npm_package_json: string;
	export const P9K_SSH: string;
	export const VSCODE_GIT_ASKPASS_MAIN: string;
	export const XDG_DATA_DIRS: string;
	export const GDK_BACKEND: string;
	export const PATH: string;
	export const npm_config_node_gyp: string;
	export const ORIGINAL_XDG_CURRENT_DESKTOP: string;
	export const DBUS_SESSION_BUS_ADDRESS: string;
	export const MAIL: string;
	export const npm_config_registry: string;
	export const FNM_DIR: string;
	export const FNM_MULTISHELL_PATH: string;
	export const KITTY_INSTALLATION_DIR: string;
	export const npm_node_execpath: string;
	export const FNM_LOGLEVEL: string;
	export const OLDPWD: string;
	export const TERM_PROGRAM: string;
	export const NODE_ENV: string;
}

/**
 * Similar to [`$env/static/private`](https://kit.svelte.dev/docs/modules#$env-static-private), except that it only includes environment variables that begin with [`config.kit.env.publicPrefix`](https://kit.svelte.dev/docs/configuration#env) (which defaults to `PUBLIC_`), and can therefore safely be exposed to client-side code.
 * 
 * Values are replaced statically at build time.
 * 
 * ```ts
 * import { PUBLIC_BASE_URL } from '$env/static/public';
 * ```
 */
declare module '$env/static/public' {
	
}

/**
 * This module provides access to runtime environment variables, as defined by the platform you're running on. For example if you're using [`adapter-node`](https://github.com/sveltejs/kit/tree/master/packages/adapter-node) (or running [`vite preview`](https://kit.svelte.dev/docs/cli)), this is equivalent to `process.env`. This module only includes variables that _do not_ begin with [`config.kit.env.publicPrefix`](https://kit.svelte.dev/docs/configuration#env) _and do_ start with [`config.kit.env.privatePrefix`](https://kit.svelte.dev/docs/configuration#env) (if configured).
 * 
 * This module cannot be imported into client-side code.
 * 
 * ```ts
 * import { env } from '$env/dynamic/private';
 * console.log(env.DEPLOYMENT_SPECIFIC_VARIABLE);
 * ```
 * 
 * > In `dev`, `$env/dynamic` always includes environment variables from `.env`. In `prod`, this behavior will depend on your adapter.
 */
declare module '$env/dynamic/private' {
	export const env: {
		SHELL: string;
		npm_command: string;
		LSCOLORS: string;
		USER_ZDOTDIR: string;
		COLORTERM: string;
		HYPRLAND_CMD: string;
		VSCODE_DEBUGPY_ADAPTER_ENDPOINTS: string;
		LESS: string;
		XDG_SESSION_PATH: string;
		TERM_PROGRAM_VERSION: string;
		FNM_ARCH: string;
		XDG_BACKEND: string;
		_P9K_TTY: string;
		NODE: string;
		npm_config_ignore_scripts: string;
		JAVA_HOME: string;
		npm_config_verify_deps_before_run: string;
		P9K_TTY: string;
		npm_config__jsr_registry: string;
		PYDEVD_DISABLE_FILE_VALIDATION: string;
		FNM_NODE_DIST_MIRROR: string;
		DESKTOP_SESSION: string;
		__ETC_PROFILE_NIX_SOURCED: string;
		KITTY_PID: string;
		HL_INITIAL_WORKSPACE_TOKEN: string;
		NO_AT_BRIDGE: string;
		XDG_SEAT: string;
		PWD: string;
		NIX_PROFILES: string;
		LOGNAME: string;
		XDG_SESSION_DESKTOP: string;
		QT_QPA_PLATFORMTHEME: string;
		XDG_SESSION_TYPE: string;
		BUNDLED_DEBUGPY_PATH: string;
		_: string;
		KITTY_PUBLIC_KEY: string;
		VSCODE_GIT_ASKPASS_NODE: string;
		MOTD_SHOWN: string;
		VSCODE_INJECTION: string;
		HOME: string;
		LANG: string;
		FNM_COREPACK_ENABLED: string;
		LS_COLORS: string;
		_JAVA_AWT_WM_NONREPARENTING: string;
		XDG_CURRENT_DESKTOP: string;
		npm_package_version: string;
		STARSHIP_SHELL: string;
		WAYLAND_DISPLAY: string;
		NIX_SSL_CERT_FILE: string;
		KITTY_WINDOW_ID: string;
		GIT_ASKPASS: string;
		XDG_SEAT_PATH: string;
		pnpm_config_verify_deps_before_run: string;
		INIT_CWD: string;
		CHROME_DESKTOP: string;
		STARSHIP_SESSION_KEY: string;
		QT_QPA_PLATFORM: string;
		npm_lifecycle_script: string;
		VSCODE_GIT_ASKPASS_EXTRA_ARGS: string;
		XDG_SESSION_CLASS: string;
		ANDROID_HOME: string;
		TERM: string;
		TERMINFO: string;
		npm_package_name: string;
		ZSH: string;
		VSCODE_NONCE: string;
		ZDOTDIR: string;
		USER: string;
		npm_config_frozen_lockfile: string;
		VSCODE_GIT_IPC_HANDLE: string;
		HYPRLAND_INSTANCE_SIGNATURE: string;
		DISPLAY: string;
		npm_lifecycle_event: string;
		SHLVL: string;
		MOZ_ENABLE_WAYLAND: string;
		PAGER: string;
		ANDROID_SDK_ROOT: string;
		_P9K_SSH_TTY: string;
		FNM_VERSION_FILE_STRATEGY: string;
		XDG_VTNR: string;
		XDG_SESSION_ID: string;
		npm_config_user_agent: string;
		PNPM_SCRIPT_SRC_DIR: string;
		npm_execpath: string;
		XDG_RUNTIME_DIR: string;
		FNM_RESOLVE_ENGINES: string;
		DEBUGINFOD_URLS: string;
		npm_package_json: string;
		P9K_SSH: string;
		VSCODE_GIT_ASKPASS_MAIN: string;
		XDG_DATA_DIRS: string;
		GDK_BACKEND: string;
		PATH: string;
		npm_config_node_gyp: string;
		ORIGINAL_XDG_CURRENT_DESKTOP: string;
		DBUS_SESSION_BUS_ADDRESS: string;
		MAIL: string;
		npm_config_registry: string;
		FNM_DIR: string;
		FNM_MULTISHELL_PATH: string;
		KITTY_INSTALLATION_DIR: string;
		npm_node_execpath: string;
		FNM_LOGLEVEL: string;
		OLDPWD: string;
		TERM_PROGRAM: string;
		NODE_ENV: string;
		[key: `PUBLIC_${string}`]: undefined;
		[key: `${string}`]: string | undefined;
	}
}

/**
 * Similar to [`$env/dynamic/private`](https://kit.svelte.dev/docs/modules#$env-dynamic-private), but only includes variables that begin with [`config.kit.env.publicPrefix`](https://kit.svelte.dev/docs/configuration#env) (which defaults to `PUBLIC_`), and can therefore safely be exposed to client-side code.
 * 
 * Note that public dynamic environment variables must all be sent from the server to the client, causing larger network requests — when possible, use `$env/static/public` instead.
 * 
 * ```ts
 * import { env } from '$env/dynamic/public';
 * console.log(env.PUBLIC_DEPLOYMENT_SPECIFIC_VARIABLE);
 * ```
 */
declare module '$env/dynamic/public' {
	export const env: {
		[key: `PUBLIC_${string}`]: string | undefined;
	}
}
