"use client"

import React from "react"
import MainContainer from "@/components/main-container"
import { DataTable } from "@/components/ui/data-table"
import { columns } from "./table/columns"
import useTargets from "@/hooks/useTargets"
import AddTargetDialog from "./dialogs/add-target-dialog"
import { useParams, useRouter } from "next/navigation"

export default function TargetsPage() {
  const { projectId }: { projectId: string } = useParams()
  const { targets } = useTargets(projectId)
  const router = useRouter()

  if (!targets?.items) return null

  return (
    <MainContainer props={{ className: "" }}>
      <AddTargetDialog />
      <DataTable
        columns={columns}
        data={targets.items}
        onRowClick={(id) => router.push(`/${projectId}/targets/${id}/overview`)}
      />
    </MainContainer>
  )
}
