   M S B A M L   `   `   `  ����Q  MSportsEventTracker.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullU/clr-namespace:SportsEventTracker.WPF.ViewModels!SportsEventTracker.WPF.ViewModels  5      ,    &SportsEventTracker.WPF.Pages.TeamsPage   5      W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      >vm/clr-namespace:SportsEventTracker.WPF.ViewModels     5        N� Title$  Teams��5       � DataContext 5      5   /SportsEventTracker.WPF.ViewModels.TeamViewModel  5   
   .��5
      � $��20q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��Teams Management��6   $��28(�5      $
��Bold"�6"   
����  �$��Center=�5      $��0,0,0,20q�5      �� 5   
    �� Orientation$ 
Horizontal=�6   $��1��6.   $��Center=�6;   $��0,0,0,20q�6X   .+�5      �� $	��300��6   $��35��6"   $��10q�6.   $��16(�5      $��5q�5      ��5      �� *NewTeamName+ �� UpdateSourceTrigger$ PropertyChanged=��� 5"      $��Add Team��6   $	��150��5#      $��35��6!   $��10q�6-   $��16(�5$      ���#28B463
���   �$��0q�5'      ��5(      �� *AddTeamCommand+5)      �� 5,   
   $��2��61   $��0,0,0,20q�6>   ��6   �� *Teams+ �� View 5-      �� 5.      .P�5/      �� $��	Team Name��6%    �� Width$	 500��6b    �� DisplayMemberBinding 68   �� *
TeamName+6b   50      52      53      54      