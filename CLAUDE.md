# CLAUDE.md

このファイルは、このリポジトリでコードを扱う際にClaude Code (claude.ai/code)にガイダンスを提供します。

## プロジェクト概要

これはUnity 6000.0.41f1プロジェクトで、プレイヤーが自動照準武器システムでウェーブごとの敵に立ち向かう3Dウェーブベースのサバイバルシューティングゲームを実装しています。

## Unity開発コマンド

### プロジェクトを開く
- Unity Hub でUnity 6000.0.41f1を使用してこのプロジェクトフォルダを開く
- メインシーンの場所: `Assets/Scenes/SampleScene.unity`

### テスト
- Unity Test Runnerでテストを実行: Window → General → Test Runner
- プレイモードテスト: Unityエディタでプレイモードに入る (Ctrl/Cmd + P)

### ビルド
- File → Build Settings からビルド
- サポートされているプラットフォームは ProjectSettings/EditorBuildSettings.asset で設定

## コードアーキテクチャ

### シングルトンマネージャーパターン
プロジェクトはグローバルな状態管理にシングルトンマネージャーを使用:
- **GameManager** (`Assets/scripts/GameManager.cs`): ゲーム状態の中央コントローラー
  - ゲームオーバー/クリア条件を管理
  - `Time.timeScale = 0f` でゲームを一時停止
  - `GameManager.Instance` でシングルトンにアクセス
- **UIManager** (`Assets/scripts/UiManager.cs`): UI状態管理
  - HP表示、ウェーブカウンター、ゲーム終了画面を処理
  - `UIManager.Instance` でシングルトンにアクセス

### コアゲームシステム

#### プレイヤーシステム
- **PlayerController** (`Assets/scripts/playercontroller.cs`):
  - Input.GetAxis (Horizontal/Vertical) による移動
  - ダメージフラッシュエフェクト付きのHP管理
  - 敵との衝突ベースのダメージ (衝突ごとに10ダメージ)
  - HP ≤ 0でGameOverをトリガー

#### 武器システム
- **WeaponSystem** (`Assets/scripts/WeaponSystems.cs`):
  - fireRateに基づいて最も近い敵に自動発射
  - GameObject.FindGameObjectsWithTag("Enemy")でターゲットを検索
  - 方向と速度を持つ弾丸をインスタンス化
- **Bullet** (`Assets/scripts/Bullet.cs`):
  - 5秒間のライフタイムを持つ直線移動
  - 敵とのトリガーベースの衝突 (25ダメージ)
  - 衝突時に自己破壊

#### 敵システム
- **Enemy** (`Assets/scripts/Enemy.cs`):
  - 正規化された方向ベクトルを使用してプレイヤーを追跡
  - ダメージフラッシュフィードバック付きのHPベースの体力
  - コルーチンによる死亡時のスムーズな縮小アニメーション
  - Playerタグを使用してターゲットを検索
- **EnemySpawner** (`Assets/scripts/EnemySpawner.cs`):
  - ウェーブベースのスポーン (ウェーブごとの敵数と総ウェーブ数が設定可能)
  - spawnRadius内のランダムな位置に敵をスポーン
  - すべての敵が倒されると自動的に次のウェーブに進行
  - 最終ウェーブ後にGameClearをトリガー

#### カメラシステム
- **CameraFollow** (`Assets/scripts/CameraFollow.cs`):
  - 設定可能なオフセットでスムーズなカメラフォロー
  - アイソメトリックビュー用の45°角度での固定回転
  - スムーズなフォローのためにLateUpdateを使用

### Unityパッケージ
主要な依存関係 (`Packages/manifest.json`を参照):
- `com.unity.inputsystem` (1.13.1): プレイヤーコントロール用の新しいInput System
- `com.unity.render-pipelines.universal` (17.0.4): レンダリング用のURP
- `com.unity.ugui` (2.0.0): UIシステム
- TextMesh Pro: テキストレンダリング (`Assets/TextMesh Pro/`にアセットを含む)

### Prefabs
- `Assets/Prefabs/Enemy.prefab`: 敵のゲームオブジェクト
- `Assets/Prefabs/Bullet.prefab`: 弾丸の発射体

### 必要なタグ
ゲームはUnityタグに依存:
- `Player`: プレイヤーの識別用
- `Enemy`: 敵の識別とターゲティング用

### シーン構造
メインシーン (`Assets/Scenes/SampleScene.unity`) には以下が含まれている必要があります:
- PlayerControllerとWeaponSystemを持つPlayerオブジェクト
- GameManagerコンポーネントを持つGameManagerオブジェクト
- UIManagerコンポーネントとUI参照を持つUIManagerオブジェクト
- EnemySpawnerコンポーネントを持つEnemySpawnerオブジェクト
- CameraFollowコンポーネントを持つCamera

## コード規約

### 命名規則
- C#スクリプトはクラス名にPascalCaseを使用 (例: `GameManager`)
- 1つの例外: `playercontroller.cs` (ファイル名は小文字だが、クラスは`PlayerController`)
- Inspectorに公開されるpublicフィールドは説明的な名前のcamelCaseを使用

### デザインパターン
- Awake()でnullチェックを行うマネージャー用のシングルトンパターン
- Unityの規約に従ったコンポーネントベースのアーキテクチャ
- アニメーション用のコルーチン (例: 敵の死亡時の縮小エフェクト)
- 視覚的フィードバック用のマテリアル色操作 (ダメージフラッシュ)

### システム間の通信
- マネージャーはシングルトンパターンで通信: `ManagerName.Instance?.MethodCall()`
- nullリファレンスエラーを防ぐためにnull条件演算子 (`?.`) を使用
- ランタイムオブジェクトクエリにはGameObject.FindGameObjectsWithTag()を使用
- コンポーネントアクセスにはGetComponent<>()を使用
